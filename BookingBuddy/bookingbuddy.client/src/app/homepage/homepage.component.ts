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
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent implements OnInit {
  signedIn: boolean = false;
  submitting: boolean = false;
  user: UserInfo | undefined;
  property_list: Property[] = [];
  numberOfPages: number = 1;
  startIndex: number = 0;
  itemsPerPage: number = 100;
  numberOfProperties: number = 0;
  private modalService: NgbModal = inject(NgbModal);

  constructor(
    private authService: AuthorizeService,
    private propertyService: PropertyAdService,
    private router: Router,
    private route: ActivatedRoute,
    private FeedbackService: FeedbackService) {
    route.queryParams.subscribe(() => {
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
            this.property_list = response as Property[];
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
            //this.property_list = this.generateRandomProperties(50);
            this.property_list = response as Property[];
            this.submitting = false;
          }
        }).catch(error => {
          this.submitting = false;
          console.error("Erro ao carregar propriedades: " + error);
        });
    }
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
    /*let modalRef = this.modalService.open(AddPropertyModal,
      {
        animation: true,
        size: 'lg',
        centered: true,
      });
    modalRef.componentInstance.onAccept = async () => {
      modalRef.close();
      await this.router.navigate(['/']);
    }*/
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
  template: `
    <div class="modal-header" aria-labelledby="acceptInviteLabel">
      <h5 class="modal-title" id="acceptInviteLabel">Adicionar Propriedade</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <p class="text-muted">Leia cuidadosamente os seguintes procedimentos.</p>
      <p>Para adicionar uma propriedade ao seu grupo de reserva deve:</p>
      <ol class="list-group list-group-numbered list-group-flush mb-2">
        <li class="list-group-item"><strong>Ir para a Página Inicial:</strong> Clique em "Aceitar" para começar a
          explorar.
        </li>
        <li class="list-group-item"><strong>(Opcional) Pesquisar e Filtrar:</strong> Encontre uma propriedade que se
          adeque às suas necessidades.
        </li>
        <li class="list-group-item"><strong>Adicionar ao Grupo:</strong> Selecione uma propriedade e clique em
          "Adicionar ao Grupo de Reserva".
        </li>
        <li class="list-group-item"><strong>Selecionar Grupo:</strong> Escolha o grupo de reserva onde deseja
          incluir a propriedade.
        </li>
        <li class="list-group-item"><strong>Concluir:</strong> A propriedade será automaticamente adicionada ao grupo
          selecionado.
        </li>
      </ol>
      <p class="mt-2"><strong>NOTA:</strong> Para cada grupo de reserva existe um máximo de 6 propriedades a escolher.
      </p>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="onClose()">Cancelar</button>
      <button type="button" class="btn btn-success" (click)="onAccept()">Aceitar</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf
  ]
})
export class FiltersModal {
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }

  protected onAccept: Function = () => {
    this.activeModal.close();
  }
}
