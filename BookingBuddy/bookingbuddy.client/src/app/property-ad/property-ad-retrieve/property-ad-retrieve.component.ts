import {ChangeDetectorRef, Component, inject, Injectable, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Property} from '../../models/property';
import {AuthorizeService} from "../../auth/authorize.service";
import {PropertyAdService} from '../property-ad.service';
import {FavoriteService} from '../../favorite-sidebar/favorite.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AmenitiesHelper} from "../../models/amenity-enum";
import {AppComponent} from '../../app.component';

import {UserInfo} from "../../auth/authorize.dto";
import {DateFilterFn, MatCalendarCellClassFunction, MatDatepickerInputEvent} from '@angular/material/datepicker';
import {OrderService} from '../../payment/order.service';
import {Group, GroupAction, GroupActionHelper} from '../../models/group';
import {GroupService} from '../../group/group.service';
import {timeout} from 'rxjs';
import {Discount} from "../../models/discount";
import {DatePipe} from '@angular/common';
import {FeedbackType} from "../../models/feedback";
import {FeedbackService} from "../../auxiliary/feedback.service";

@Component({
  selector: 'app-property-ad-retrieve',
  templateUrl: './property-ad-retrieve.component.html',
  styleUrl: './property-ad-retrieve.component.css',
})

@Injectable({
  providedIn: 'root'
})


export class PropertyAdRetrieveComponent implements OnInit {
  property: Property | undefined;
  reservarPropriedadeForm!: FormGroup;
  reservarPropriedadeFailed: boolean;
  signedIn: boolean = false;
  isPropertyInFavorites: boolean = false;
  blockedDates: Date[] = [];
  discounts: Discount[] = [];
  discountDates: Date[] = [];
  checkInDate: Date | undefined;
  checkOutDate: Date | undefined;
  calendarMaxDate: Date = new Date(8640000000000000);
  maxDate: Date = this.calendarMaxDate;
  pricesMap: Map<number, number> = new Map<number, number>();
  protected readonly AmenitiesHelper = AmenitiesHelper;
  protected isLandlord: boolean = false;
  group_list: Group[] = [];
  protected user: UserInfo | undefined;
  submitting: boolean = false;
  errors: string[] = [];
  selected_group_list: Group[] = [];
  private feedbackService: FeedbackService = inject(FeedbackService);
  @ViewChild('myModalClose') modalClose: any;

  constructor(private datePipe: DatePipe, private cdref: ChangeDetectorRef, private groupService: GroupService, private appComponent: AppComponent, private propertyService: PropertyAdService,
              private route: ActivatedRoute, private formBuilder: FormBuilder, private authService: AuthorizeService, private orderService: OrderService, private router: Router, private favoriteService: FavoriteService) {
    this.reservarPropriedadeFailed = false;

    this.reservarPropriedadeForm = this.formBuilder.group({
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required],
      numberOfGuests: ['1', Validators.required]
    });

    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        this.checkPropertyIsFavorite();
        if (isSignedIn) {
          this.authService.user().forEach(user => {
            this.user = user
            if (user.roles.includes('landlord') || user.roles.includes('admin')) {
              this.isLandlord = true;
            }
            this.loadUserGroups();
          });
        }
      });
  }

  ngOnInit() {
    this.propertyService.getProperty(this.route.snapshot.params['id']).forEach(
      response => {
        if (response) {
          console.log(response);
          this.property = response as Property;
          this.loadDiscounts();
          this.loadBlockedDates();
        }
      }).catch(
      error => {
        // TODO return error message
      }
    );
  }

  private loadUserGroups() {
    this.groupService.getGroupsByUserId(this.user!.userId).pipe(timeout(10000)).forEach(groups => {
      this.group_list = groups;
      this.group_list = this.group_list.filter(
        group => group.members.length <= this.property!.maxGuestsNumber
          && !group.properties.find(g => g.propertyId === this.property!.propertyId)
          && GroupActionHelper.parse(group.groupAction) === GroupAction.none);
      this.submitting = false;
    }).catch(error => {
      this.errors.push(error.error);
      this.submitting = false;
    });
  }

  loadBlockedDates() {
    if (this.property) {
      this.propertyService.getPropertyBlockedDates(this.property.propertyId)
        .forEach(
          (dateRanges: any[]) => {

            this.blockedDates = [];

            dateRanges.forEach(dateRange => {
              const startDate = new Date(dateRange.start);
              const endDate = new Date(dateRange.end);

              const currentDate = new Date(startDate);
              while (currentDate <= endDate) {
                this.blockedDates.push(new Date(currentDate));
                currentDate.setDate(currentDate.getDate() + 1);
              }
            });

          }).catch(error => {
          console.error('Erro ao carregar intervalos de datas bloqueadas:', error);
        }
      );
    }
  }

  guestNumbers(): number[] {
    const maxGuests = this.property?.maxGuestsNumber || 1;
    return Array.from({length: maxGuests}, (_, index) => index + 1);
  }

  loadDiscounts() {
    if (this.property) {
      this.propertyService.getPropertyDiscounts(this.property.propertyId)
        .forEach(
          (dateRanges: any[]) => {
            this.discounts = [];

            dateRanges.forEach(dateRange => {
              const discount: Discount = {
                discountId: dateRange.discountId,
                startDate: dateRange.startDate,
                endDate: dateRange.endDate,
                discountAmount: dateRange.discountAmount,
                dates: []
              };

              const startDate = new Date(dateRange.startDate);
              const endDate = new Date(dateRange.endDate);

              const currentDate = new Date(startDate);
              while (currentDate <= endDate) {
                discount.dates.push(new Date(currentDate));
                this.discountDates.push(new Date(currentDate));
                currentDate.setDate(currentDate.getDate() + 1);
              }

              this.discounts.push(discount);
            });

          }).catch(error => {
          console.error('Erro ao carregar intervalos de datas bloqueadas:', error);
        }
      );
    }
  }

  discountClass: MatCalendarCellClassFunction<Date> = (cellDate, view) => {
    if (this.discountDates.some(discountDate => this.isSameDay(cellDate, discountDate))) {
      return 'discount-date-class';
    }

    return '';
  };


  protected filterDates: DateFilterFn<any> = (date: Date | null) => {
    if (!date) return false;
    let today = new Date();
    let dateIntervalValid: boolean = (() => {
      let tomorrow = new Date(date);
      tomorrow.setDate(tomorrow.getDate() + 1);
      let previousDate = new Date(date);
      previousDate.setDate(previousDate.getDate() - 1);
      return !((this.isSameDay(today, previousDate) && this.blockedDates.some(blockedDate => this.isSameDay(tomorrow, blockedDate)))
        || (this.blockedDates.some(blockedDate => this.isSameDay(previousDate, blockedDate)) && this.blockedDates.some(blockedDate => this.isSameDay(tomorrow, blockedDate))));
    })();
    return date >= today && !this.blockedDates.some(blockedDate => this.isSameDay(date, blockedDate)) && dateIntervalValid;
  }


  private isSameDay(date1: Date, date2: Date): boolean {
    return date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate();
  }

  onDateChange(event: MatDatepickerInputEvent<Date>, type: 'start' | 'end'): void {
    if (type === 'start' && event.value) {
      this.checkInDate = event.value;
      this.updateMaxDate();
    } else if (event.value) {
      this.checkOutDate = event.value;
    }
    this.cdref.detectChanges();
  }

  updateMaxDate(): void {
    if (this.checkInDate) {
      const nextBlockedDate = this.blockedDates.find(date => date > this.checkInDate!);

      if (nextBlockedDate) {
        this.maxDate = nextBlockedDate;
      } else {
        this.maxDate = this.calendarMaxDate;
      }
    }
  }

  public selectGroup(group: Group) {
    const index = this.selected_group_list.indexOf(group);
    if (!this.selected_group_list.includes(group)) {
      this.selected_group_list.push(group);
    } else {
      this.selected_group_list.splice(index, 1);
    }

  }


  clearDates() {
    this.reservarPropriedadeForm.patchValue({
      checkIn: null,
      checkOut: null
    });

    this.checkInDate = this.checkOutDate = undefined;
    this.maxDate = this.calendarMaxDate;
    this.cdref.detectChanges();
  }

  /*calcularDiferencaDias(): number {
    const checkInDateString: string = this.reservarPropriedadeForm.get('checkIn')?.value;
    const checkOutDateString: string = this.reservarPropriedadeForm.get('checkOut')?.value;
    const checkInDate: Date = new Date(checkInDateString);
    const checkOutDate: Date = new Date(checkOutDateString);

    if(!this.checkInDate || !this.checkOutDate|| this.checkOutDate <= this.checkInDate) {
      return 1;
    }

    const diferencaMilissegundos: number = this.checkOutDate.getTime() - this.checkInDate.getTime();
    const diferencaDias: number = diferencaMilissegundos / (1000 * 60 * 60 * 24);

    return diferencaDias;
  }*/

  addToFavorites() {
    if (this.property) {
      this.favoriteService.addToFavorites(this.property?.propertyId).forEach(
        response => {
          if (response) {
            this.isPropertyInFavorites = true;
          }
        })
        .then(() => {
          this.feedbackService.setFeedback({
            feedback: 'Propriedade adicionada aos favoritos com sucesso.',
            type: FeedbackType.INFO
          });
        })
        .catch(
          error => {
            console.error('Erro ao adicionar ao favoritos:', error);
          }
        );
    }
  }

  removeFromFavorites() {
    if (this.property) {
      this.favoriteService.removeFromFavorites(this.property?.propertyId).forEach(
        response => {
          if (response) {
            this.isPropertyInFavorites = false;
          }
        })
        .then(() => {
          this.feedbackService.setFeedback({
            feedback: 'Propriedade removida dos favoritos com sucesso.',
            type: FeedbackType.INFO
          });
        })
        .catch(
        error => {
          console.error('Erro ao adicionar ao favoritos:', error);
        }
      );
    }
  }

  public addPropertyToGroup() {
    if (this.property && this.selected_group_list.length > 0) {
      const propertyId = this.property.propertyId;

      this.selected_group_list.forEach(group => {

        this.groupService.addPropertyToGroup(group.groupId, propertyId).forEach(response => {
          if (response) {
            //console.log(response);
          }
        }).then(() => {
          this.feedbackService.setFeedback({
            feedback: 'Propriedade adicionada ao grupo com sucesso.',
            type: FeedbackType.SUCCESS
          });
        });
      });


      this.modalClose.nativeElement.click();

      //   this.router.navigateByUrl('/groups?groupId=' + this.selected_group_list[0].groupId);
      this.selected_group_list = [];
    }
  }

  calcularTotalDesconto() {
    const selectedDates: Date[] = [];
    this.pricesMap = new Map();
    if (this.property && this.checkInDate && this.checkOutDate) {
      const currentDate = new Date(this.checkInDate);
      while (currentDate < this.checkOutDate) {
        selectedDates.push(new Date(currentDate));
        currentDate.setDate(currentDate.getDate() + 1);
      }

      selectedDates.forEach(selectedDate => {
        const matchingDiscounts = this.discounts.filter(discount =>
          discount.dates.some(date => this.isSameDay(date, selectedDate))
        );

        if (matchingDiscounts.length > 0) {
          matchingDiscounts.forEach(discount => {
            const newPrice = this.priceWithDiscount(discount.discountAmount);
            const currentCount = this.pricesMap.get(newPrice) || 0;
            this.pricesMap.set(newPrice, currentCount + 1);
          });
        } else {
          const currentCount = this.pricesMap.get(this.property?.pricePerNight!) || 0;
          this.pricesMap.set(this.property?.pricePerNight!, currentCount + 1);
        }
      });

      let totalPrice = 0;

      this.pricesMap.forEach((count, price) => {
        const aux = count * price;
        totalPrice += aux;
      });
      return Math.round(((totalPrice) + Number.EPSILON) * 100) / 100;
    }

    return 0;
  }

  priceWithDiscount(discountAmount: number): number {
    if (this.property) {
      const discountMultiplier = 1 - discountAmount / 100;
      return Math.round(((this.property.pricePerNight * discountMultiplier) + Number.EPSILON) * 100) / 100
    }
    return 0;
  }

  formatPricesMap(): string[] {
    const formattedStrings: string[] = [];

    this.pricesMap.forEach((count, price) => {
      formattedStrings.push(`${price}€ x ${count} noites - ${Math.round(((price * count) + Number.EPSILON) * 100) / 100}€`);
    });

    return formattedStrings;
  }

  checkPropertyIsFavorite() {
    if (this.signedIn) {
      this.propertyService.isPropertyInFavorites(this.route.snapshot.params['id']).forEach(
        result => {
          this.isPropertyInFavorites = result;
        }).catch(
        error => {
          console.error('Erro ao verificar se a propriedade está nos favoritos:', error);
        }
      );
    }
  }

  public reservar(_: any) {
    this.reservarPropriedadeFailed = false;
    const numberOfGuests: number = this.reservarPropriedadeForm.get('numberOfGuests')?.value;
    const checkInDate: Date = this.reservarPropriedadeForm.get('checkIn')?.value;
    const checkOutDate: Date = this.reservarPropriedadeForm.get('checkOut')?.value;
    console.log("Check-in: " + checkInDate);
    console.log("Check-out: " + checkOutDate);

    // TODO: verificar se datas sao validas antes de fazer a order
    this.router.navigate(['/transaction-handler'], {
      queryParams: {
        propertyId: this.property?.propertyId,
        startDate: this.datePipe.transform(checkInDate, 'yyyy-MM-dd'),
        endDate: this.datePipe.transform(checkOutDate, 'yyyy-MM-dd'),
        numberOfGuests: numberOfGuests,
        orderType: 'booking'
      }
    });
  }
}
