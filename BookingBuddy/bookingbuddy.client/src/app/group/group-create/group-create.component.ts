import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject, interval, map, Observable, of, Subject, timeout } from 'rxjs';
import { GroupService } from '../group.service';
import { AuthorizeService } from '../../auth/authorize.service';
import { UserInfo } from "../../auth/authorize.dto";
import { Group, GroupCreate } from "../../models/group";
import { GroupName } from "./group-name-step/group-name-step.component";
import { GroupMembers } from "./group-members-step/group-members-step.component";
import { AppComponent } from '../../app.component';

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

  constructor(private appComponent: AppComponent,
    private authService: AuthorizeService,
    private router: Router,
    private groupService: GroupService) {
    this.errors = [];
    this.appComponent.showChat = false;
  }

  ngOnInit(): void {
    this.authService.user().forEach(user => {
      this.user = user;
    });
    this.onStepChange().forEach(step => {
      this.currentStep = step;
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
      propertyId: '',
      members: []
      };
      this.groupService.createGroup(newGroup).forEach(success => {
        if (success) {
          this.router.navigateByUrl('/');
        }
      }
      ).catch(() => {
        this.errors.push('Erro ao criar grupo.');
        this.submitting = false;
        return of(null);
      });
  }
}
