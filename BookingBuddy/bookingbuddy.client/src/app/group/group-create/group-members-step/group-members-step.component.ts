import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'app-group-members-step',
  templateUrl: './group-members-step.component.html',
  styleUrl: './group-members-step.component.css'
})
export class GroupMembersStepComponent {
  @Input() errors: string[] = [];
  @Input() groupMembers: GroupMembers | undefined;
  @Output() groupMembersValid = new EventEmitter<boolean>();
  @Output() groupMembersSubmit = new EventEmitter<GroupMembers>();
  protected currentGroupMembers: GroupMembers = { members: [] };
  protected groupMembersForm = this.formBuilder.group({
    members: ['', [Validators.required, Validators.email]]
  });
  addOnBlur = true;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  emails: string[] = [];
  emailControl: FormControl = new FormControl('', [Validators.required, Validators.email]);
  invalidEmailError = false;
  constructor(private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    this.groupMembersValid.emit(true);
  }



  removeEmail(email: string): void {
    this.currentGroupMembers.members = this.currentGroupMembers.members.filter(item => item !== email);
  }

  save(event: any): void {
    const email = event.target.value.trim();
    if (event.key === 'Enter' || event.key === 'Space') {
      if (email && this.emailControl.valid && !this.emails.includes(email)) {
        this.emails.push(email); 
        event.target.value = ''; 
        this.currentGroupMembers.members.push(email);
        this.groupMembersSubmit.emit(this.currentGroupMembers);
      }
    }

  }


  remove(index: number): void {
    if (index >= 0) {
      this.emails.splice(index, 1);
    }
  }

  get groupMembersFormField() {
    return this.groupMembersForm.get('members');
  }
}

export interface GroupMembers {
  members: string[]
}
