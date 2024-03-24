import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {BehaviorSubject, interval, map, Observable, of, Subject, timeout} from 'rxjs';
import {GroupService} from '../group.service';
import {AuthorizeService} from '../../auth/authorize.service';
import {UserInfo} from "../../auth/authorize.dto";
import {Group, GroupCreate} from "../../models/group";
import {GroupName} from "./group-name-step/group-name-step.component";
import {GroupMembers} from "./group-members-step/group-members-step.component";

import {PropertyAdService} from '../../property-ad/property-ad.service';
import {FooterService} from "../../auxiliary/footer.service";

@Component({
  selector: 'app-group-create',
  templateUrl: './group-create.component.html',
  styleUrl: './group-create.component.css'
})
export class GroupCreateComponent implements OnInit {
  protected user: UserInfo | undefined;
  protected submitting: boolean = false;
  protected errors: string[] = [];
  protected currentStep: number = 0;
  protected readonly numberOfSteps: number = 2;
  protected step: Subject<number> = new BehaviorSubject(0);

  protected onStepChange() {
    return this.step.asObservable();
  }

  // Group Name
  protected groupName: GroupName | undefined;
  protected isGroupNameValid: boolean = false;

  // Group Members
  protected groupMembers: GroupMembers | undefined;
  protected isGroupMembersValid: boolean = false;

  // Property
  protected propertyId: string = '';

  constructor(private propertyService: PropertyAdService,
              private authService: AuthorizeService,
              private route: ActivatedRoute,
              private router: Router,
              private footerService: FooterService,
              private groupService: GroupService) {
    this.errors = [];
    this.footerService.hideFooter();
  }

  ngOnInit(): void {

    this.authService.user().forEach(user => {
      this.user = user;
    });

    this.onStepChange().forEach(step => {
      this.currentStep = step;
    });

    this.route.queryParams.forEach(params => {
      if (params['propertyId']) {
        this.propertyService.getProperty(params['propertyId']).forEach(response => {
          if (response) {
            this.propertyId = params['propertyId'];
          }
        }).catch(error => {
            this.router.navigate(['group-booking']);
          }
        );
      }
    });
  }

  previousStep() {
    if (this.currentStep > 0) {
      switch (this.currentStep) {
        case 1:
          this.groupName = undefined;
          this.isGroupNameValid = false;
          break;
        case 2:
          this.groupMembers = undefined;
          this.isGroupMembersValid = false;
          break;
      }
      this.step.next(this.currentStep - 1);
    }
  }

  nextStep() {
    if (this.currentStep < this.numberOfSteps) {
      this.step.next(this.currentStep + 1);
    }
  }

  get currentCompletePercentage() {
    return ((this.currentStep) / this.numberOfSteps) * 100;
  }

  setGroupName($event: GroupName | undefined) {
    this.groupName = $event;
  }

  groupNameValid($event: boolean) {
    this.isGroupNameValid = $event;
  }

  setGroupMembers($event: GroupMembers | undefined) {
    this.groupMembers = $event;
  }

  groupMembersValid($event: boolean) {
    this.isGroupMembersValid = $event;
  }

  isValid(): boolean {
    switch (this.currentStep) {
      case 1:
        return this.isGroupNameValid;
      case 2:
        return this.isGroupMembersValid;
      default:
        return true;
    }
  }

  public create(_: any) {
    this.submitting = true;
    this.errors = [];

    const newGroup: GroupCreate = {
      name: this.groupName?.name ?? '',
      propertyId: this.propertyId,
      members: this.groupMembers?.members ?? []
    };
    this.groupService.createGroup(newGroup).forEach(group => {
        if (group) {
          this.submitting = true;
          this.router.navigate(['/groups'], { queryParams: { groupId: group.groupId } }).then(() => {
              this.submitting = false;
            }
          );
        }
      }
    ).catch(() => {
      this.errors.push('Erro ao criar grupo.');
      this.submitting = false;
      return;
    });
  }
}
