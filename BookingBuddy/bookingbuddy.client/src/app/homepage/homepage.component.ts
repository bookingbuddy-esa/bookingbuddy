import {Component, OnInit, inject} from '@angular/core';
import {AuthorizeService} from '../auth/authorize.service';
import {UserInfo} from '../auth/authorize.dto';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import {Property} from '../models/property';
import {PropertyAdService} from '../property-ad/property-ad.service';
import {FeedbackService} from "../auxiliary/feedback.service";
import {timeout} from "rxjs";
import { NgbActiveModal, NgbDatepicker, NgbDatepickerModule, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { AuxiliaryModule } from '../auxiliary/auxiliary.module';
import { CommonModule, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AmenitiesHelper } from '../models/amenity-enum';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent implements OnInit {
  signedIn: boolean = false;
  submitting: boolean = false;
  propertiesFiltered: boolean = false;
  searchResult: boolean = false;
  user: UserInfo | undefined;
  property_list: Property[] = [];
  propertyFilteredList: Property[] = [];
  numberOfPages: number = 1;
  startIndex: number = 0;
  itemsPerPage: number = 100;
  numberOfProperties: number = 0;

  amenitiesFilter: string[] = [];
  roomsFilter: number | undefined;
  guestsFilter: number | undefined;
  minPriceFilter: number | undefined;
  maxPriceFilter: number | undefined;
  private modalService: NgbModal = inject(NgbModal);

  constructor(
    private authService: AuthorizeService,
    private propertyService: PropertyAdService,
    private router: Router,
    private route: ActivatedRoute,
    private FeedbackService: FeedbackService) {
    route.queryParams.subscribe(() => {
      this.clearFilters();
      this.loadProperties();
    });
  }

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(isSignedIn => {
      this.signedIn = isSignedIn;
      if (isSignedIn) {
        this.authService.user().forEach(user => this.user = user);
      }
    });

    this.submitting = true;
    this.loadProperties();
  }

  countProperties() {
    return new Promise<void>((resolve, reject) => {
      this.propertyService.getPropertiesCount().forEach(response => {
        if (response) {
          this.numberOfProperties = response as number;
          this.numberOfPages = Math.ceil(this.numberOfProperties / this.itemsPerPage);
          resolve();
        } else {
          reject("Error fetching properties count");
        }
      });
    });
  }

  loadProperties() {
    
    var search = this.route.snapshot.queryParams['search'];

    if (search) {
      this.submitting = true;
      this.propertyService.getPropertiesSearch(search, this.itemsPerPage, this.startIndex)
        .pipe(timeout(10000))
        .forEach(response => {
          if (response) {
            this.searchResult = true;
            if(response.length != 0){
              this.property_list = response as Property[];
              this.propertyFilteredList = this.property_list;
            }

            this.submitting = false;
          }
        }).catch(error => {
          this.submitting = false;
          console.error("Erro ao carregar propriedades: " + error);
        });
    } else {
      this.propertyService.getProperties(this.itemsPerPage, this.startIndex)
        .pipe(timeout(10000))
        .forEach(response => {
          if (response) {
            this.property_list = response as Property[];
            this.propertyFilteredList = this.property_list;
            this.submitting = false;
            console.log(this.property_list);
          }
        }).catch(error => {
          this.submitting = false;
          console.error("Erro ao carregar propriedades: " + error);
        });
       
    }
    
  }

  clearFilters() {
    this.minPriceFilter = undefined;
    this.maxPriceFilter = undefined;
    this.roomsFilter = undefined;
    this.guestsFilter = undefined;
    this.amenitiesFilter = [];
  }

  updateItemsPerPage(value: number) {
    this.itemsPerPage = value;
    this.countProperties().then(() => this.loadProperties());
  }

  setPage(page: number) {
    if (page < 1 || page > this.numberOfPages || this.startIndex === page - 1) {
      return;
    }

    this.startIndex = page - 1;
    this.loadProperties();
  }

  previousPage() {
    if (this.startIndex > 0) {
      console.log("Previous page -> " + this.startIndex)
      this.startIndex -= 1;
      console.log("Depois: " + this.startIndex)
      this.loadProperties();
    }
  }

  nextPage() {
    if (this.startIndex < this.numberOfPages - 1) {
      console.log("Next page -> " + this.startIndex)
      this.startIndex += 1;
      console.log("Depois: " + this.startIndex)
      this.loadProperties();
    }
  }

  showFilterModal() {
    let modalRef = this.modalService.open(FiltersModal,
      {
        animation: true,
        size: 'lg',
        centered: true,
      });
    modalRef.componentInstance.minPrice = this.minPriceFilter;
    modalRef.componentInstance.maxPrice = this.maxPriceFilter;
    modalRef.componentInstance.roomsNumber = this.roomsFilter;
    modalRef.componentInstance.guestsNumber = this.guestsFilter;
    modalRef.componentInstance.selectedAmenities = this.amenitiesFilter;
    modalRef.componentInstance.onAccept = async () => {
      modalRef.close();
      this.roomsFilter = modalRef.componentInstance.roomsNumber;
      this.guestsFilter = modalRef.componentInstance.guestsNumber;
      this.minPriceFilter = modalRef.componentInstance.minPrice;
      this.maxPriceFilter = modalRef.componentInstance.maxPrice;
      this.amenitiesFilter = modalRef.componentInstance.selectedAmenities;
      this.applyFilters();
    }
  }


  applyFilters() {
    this.propertyFilteredList = this.property_list

    if (this.roomsFilter) {
      this.propertyFilteredList = this.propertyFilteredList.filter(property => property.roomsNumber >= this.roomsFilter!);
    }

    if (this.guestsFilter) {
      this.propertyFilteredList = this.propertyFilteredList.filter(property => property.maxGuestsNumber >= this.guestsFilter!);
    }

    if (this.minPriceFilter) {
      this.propertyFilteredList = this.propertyFilteredList.filter(property => property.pricePerNight >= this.minPriceFilter!);
    }

    if (this.maxPriceFilter) {
      this.propertyFilteredList = this.propertyFilteredList.filter(property => property.pricePerNight <= this.maxPriceFilter!);
    }

    if (this.amenitiesFilter.length > 0) {
      this.propertyFilteredList = this.propertyFilteredList.filter(property =>
        this.amenitiesFilter.every(amenity =>
          property.amenities!.some(propAmenity => propAmenity.name === amenity)
        )
      );
    }

    /*if(this.propertyFilteredList != this.property_list){
      this.propertiesFiltered = true;
    }*/
  }

  orderAscending() {
    if(this.propertyFilteredList.length > 0){
      this.propertyFilteredList.sort((a, b) => a.pricePerNight - b.pricePerNight);
    }
  }

  orderDescending() {
    if(this.propertyFilteredList.length > 0){
      this.propertyFilteredList.sort((a, b) => b.pricePerNight - a.pricePerNight);
    }

    if (this.amenitiesFilter.length > 0) {
      this.propertyFiltredList = this.propertyFiltredList.filter(property =>
        this.amenitiesFilter.every(amenity =>
          property.amenities!.some(propAmenity => propAmenity.name === amenity)
        )
      );
    }
  }

  getPages(): number[] {
    return Array.from({length: this.numberOfPages}, (_, i) => i + 1);
  }


  /* TODO: este codigo vai ser para gerar propriedades para o vídeo
  houseNames: string[] = [
    "Casa Azul",
    "Casa do Sol",
    "Villa Mar",
    "Chalé da Montanha",
    "Retiro à Beira-Mar",
    "Rancho Sereno",
    "Casa da Vinha",
    "Casa da Praia",
    "Chalé Aconchegante",
    "Refúgio Tranquilo",
    "Vivenda Encantada",
    "Serenidade à Beira do Lago",
    "Casa da Floresta",
    "Casa da Montanha",
    "Villa Romântica",
    "Casa da Colina",
    "Oásis Tropical",
    "Refúgio na Serra",
    "Casa do Campo",
    "Casa da Cascata",
  ];

  locations: string[] = [
    "Lisboa",
    "Porto",
    "Faro",
    "Braga",
    "Coimbra",
    "Aveiro",
    "Albufeira",
    "Évora",
    "Sintra",
    "Guimarães",
    "Setúbal",
    "Viana do Castelo",
    "Ponta Delgada",
    "Viseu",
    "Tavira",
    "Funchal",
    "Vila Nova de Gaia",
    "Portimão",
    "Tomar",
    "Cascais",
  ];

  availableImages: string[] = [
    "https://a0.muscache.com/im/pictures/miso/Hosting-31508919/original/f5cd57a3-b42d-4211-a73c-047c6cc2fc13.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/cca0407a-df09-4907-8a6b-0a9007fbf0c7.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/a205644d-7e0d-44d4-a214-82b0f05b34bc.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/7a2f288c-ee13-4d8b-9c21-85ae624957d8.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/fe5ff38b-d386-46b2-b9f8-f36d18fcdaad.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-668106382987841521/original/1e0e68df-caff-417d-888d-082fce817f72.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-37227186/original/3d8a4ff1-d586-417a-92ba-e6fd39c4c0be.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/prohost-api/Hosting-939248182448078073/original/6d3e77d9-f943-426a-8fb7-044dfc096e47.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-40812353/original/a32105de-3961-459d-a9e8-9a4c43715161.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/71b35a78-db84-4a86-bdef-60d9acd4999d.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/b0086747-fb9d-4a40-bb75-a58f019a2f2f.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/23693583-215d-415c-9565-78982adc880c.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/0d705ee4-8713-4833-b6da-d02f4edee146.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/airflow/Hosting-661337940143552821/original/82b9a204-3221-4621-a74d-8b34416b6366.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/44408781/8c2fa590_original.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-47328735/original/a180afb2-d529-41d3-8e93-1ac11bb6238c.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/45416102/efc346b3_original.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-915784153230738025/original/c2007a57-f876-454c-9744-eb293592cd46.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-771482317449548821/original/9e5f491f-b863-4ed7-8167-a2684c713362.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-19887889/original/d45bfb48-dde3-46ef-bf72-d26cd9b3a527.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/84501969/7bd1140a_original.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/3ca23206-4975-4b80-b6f7-8da0e46d0dc4.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/313c7d97-b988-4cfb-be89-3ebf33b8970f.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-863736680857523787/original/f54e9f33-783b-4969-bdc4-b588ecce2971.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-947205134370786244/original/019caa2d-d335-4031-9d88-79fd4f686b20.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/9b5e2eff-ca68-4f22-adb0-029a33e466aa.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/4c0743d8-df89-4fd6-9c4b-4238a365368c.jpg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-910835692457376095/original/c3ac815c-8828-4ea7-a6a1-eb90f32e026d.jpeg?im_w=720",
    "https://a0.muscache.com/im/pictures/miso/Hosting-53875868/original/e87df14d-26b0-48d6-a221-5146cd27aa88.jpeg?im_w=720"
  ];


  getRandomNumber(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  getRandomImageUrl(): string {
    for (let i = 0; i < 10; i++) {
      this.availableImages.sort(() => Math.random() - 0.5);
    }

    return this.availableImages[0];
  }

  generateRandomProperties(count: number): Property[] {
    const properties: Property[] = [];

    for (let i = 0; i < count; i++) {
      const property: Property = {
        propertyId: `property${i + 1}`,
        applicationUserId: `user${i + 1}`,
        name: this.houseNames[this.getRandomNumber(0, this.houseNames.length - 1)],
        location: this.locations[this.getRandomNumber(0, this.locations.length - 1)],
        description: `Description for ${this.houseNames[i]}`,
        maxGuestsNumber: this.getRandomNumber(1, 10),
        roomsNumber: this.getRandomNumber(1, 5),
        pricePerNight: this.getRandomNumber(95, 350),
        amenities: [],
        amenityIds: [],
        imagesUrl: [
          this.getRandomImageUrl()
        ]
      };

      properties.push(property);
    }

    return properties;
  }*/


}


@Component({
  standalone: true,
  selector: 'add-property-modal',
  styleUrl: 'homepage.component.css',
  template: `
    <div class="modal-header" aria-labelledby="acceptInviteLabel">
      <h4 class="modal-title" id="acceptInviteLabel">Filtrar Propriedades</h4>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <h5><strong>Intervalo de Preços</strong></h5>
      <div class="row">
        <div class="col-md-6">
          <input type="number" class="form-control" id="minPrice" name="minPrice" [(ngModel)]="minPrice" min="0" max="9999" placeholder="Preço Mínimo">
        </div>
        <div class="col-md-6">
          <input type="number" class="form-control" id="maxPrice"  name="maxPrice" [(ngModel)]="maxPrice" min="0" max="9999" placeholder="Preço Máximo">
        </div>
      </div>
      <br>
      <h5><strong>Espaço</strong></h5>
      <h6><strong>Quartos</strong></h6>
      <div class="row">
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': roomsNumber === undefined, 'text-white': roomsNumber === undefined}" (click)="updateRoomsNumber(undefined)">Qualquer</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': roomsNumber === 1, 'text-white': roomsNumber === 1}" (click)="updateRoomsNumber(1)">1</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': roomsNumber === 2, 'text-white': roomsNumber === 2}" (click)="updateRoomsNumber(2)">2</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': roomsNumber === 3, 'text-white': roomsNumber === 3}"(click)="updateRoomsNumber(3)">3</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': roomsNumber === 4, 'text-white': roomsNumber === 4}" (click)="updateRoomsNumber(4)">4</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': roomsNumber === 5, 'text-white': roomsNumber === 5}" (click)="updateRoomsNumber(5)">5</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': roomsNumber === 6, 'text-white': roomsNumber === 6}" (click)="updateRoomsNumber(6)">6+</button>
        </div>
      </div>
      <br>
       <h6><strong>Numero Máximo de Hóspedes</strong></h6>
      <div class="row">
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': guestsNumber === undefined, 'text-white': guestsNumber=== undefined}"  (click)="updateGuestsNumber(undefined)">Qualquer</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': guestsNumber === 1, 'text-white': guestsNumber === 1}" (click)="updateGuestsNumber(1)">1</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': guestsNumber === 2, 'text-white': guestsNumber=== 2}" (click)="updateGuestsNumber(2)">2</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': guestsNumber === 3, 'text-white': guestsNumber === 3}" (click)="updateGuestsNumber(3)">3</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': guestsNumber === 4, 'text-white': guestsNumber === 4}" (click)="updateGuestsNumber(4)">4</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': guestsNumber === 5, 'text-white': guestsNumber === 5}" (click)="updateGuestsNumber(5)">5</button>
        </div>
        <div class="col">
          <button type="button" class="btn btn-outline-dark rounded-pill" [ngClass]="{'btn-dark': guestsNumber === 6, 'text-white': guestsNumber === 6}" (click)="updateGuestsNumber(6)">6+</button>
        </div>
      </div>
      <br>
      <h5><strong>Comodidades</strong></h5>



      <div class="row">
        <div class="col-md-4" *ngFor="let amenity of AmenitiesHelper.getAmenities()">
          <div class="amenity my-3 p-2 rounded d-flex flex-row align-items-center"
            (click)="selectAmenity(amenity)" [ngClass]="{'amenity-selected': isSelected(amenity)}">
            <span class="w-25 text-center material-symbols-outlined">{{ AmenitiesHelper.getAmenityIcon(amenity) }}</span>
            <span class="flex-grow-1 fw-bolder">{{ AmenitiesHelper.getAmenityDisplayName(amenity) }}</span>
          </div>
        </div>
      </div>




    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-dark text-white" (click)="clear()">Limpar Filtros</button>
      <button type="button" class="btn btn-success" (click)="onAccept()">Aplicar</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    FormsModule,
    CommonModule,
    NgIf
  ]
})
export class FiltersModal {
  minPrice: number | undefined; 
  maxPrice: number | undefined;
  roomsNumber: number | undefined;
  guestsNumber: number | undefined;

  protected readonly AmenitiesHelper = AmenitiesHelper;
  protected selectedAmenities: string[] = [];

  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }

  updateRoomsNumber(num: number | undefined) {
    this.roomsNumber = num;
  }

  updateGuestsNumber(num: number | undefined) {
    this.guestsNumber = num;
  }

  selectAmenity(amenity: string) {
    const index = this.selectedAmenities.indexOf(amenity);
    if (index !== -1) {
      this.selectedAmenities.splice(index, 1);
    } else {
      this.selectedAmenities.push(amenity);
    }
  }

  isSelected(amenity: string) {
    return this.selectedAmenities.includes(amenity);
  }

  clear() {
    this.maxPrice = undefined;
    this.minPrice = undefined;
    this.roomsNumber = undefined;
    this.guestsNumber = undefined;
    this.selectedAmenities = [];
    this.onAccept();
  }

  protected onAccept: Function = () => {
    this.activeModal.close();
  }
}
