import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

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
  protected currentGroupMembers: GroupMembers | undefined;
  protected groupMembersForm = this.formBuilder.group({
    members: ['', [Validators.required, Validators.email]]
  });

  constructor(private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    if (this.groupMembers) {
      this.currentGroupMembers = this.groupMembers;
      this.groupMembersForm.get('members')?.setValue(this.groupMembers.members.join(', '));
    }

    this.groupMembersForm.valueChanges.forEach(() => {
      this.groupMembersValid.emit(this.groupMembersForm.valid);
      const groupMembersFieldValue = this.groupMembersFormField?.value;
      if (groupMembersFieldValue) {
        this.currentGroupMembers = {
          members: groupMembersFieldValue.split(',').map(email => email.trim())
        };
        this.groupMembersSubmit.emit(this.currentGroupMembers);
      }
    })
  }

  save(event: any): void {
    console.log("You entered: ", event.target.value);
  }

  get groupMembersFormField() {
    return this.groupMembersForm.get('members');
  }
}

export interface GroupMembers {
  members: string[]
}
