import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";

@Component({
  selector: 'app-group-name-step',
  templateUrl: './group-name-step.component.html',
  styleUrl: './group-name-step.component.css'
})
export class GroupNameStepComponent implements OnInit {
  @Input() errors: string[] = [];
  @Input() groupName: GroupName | undefined;
  @Output() groupNameValid = new EventEmitter<boolean>();
  @Output() groupNameSubmit = new EventEmitter<GroupName>();
  protected currentGroupName: GroupName | undefined;
  protected groupNameForm = this.formBuilder.group({
    name: ['', Validators.required, Validators.minLength(3), Validators.maxLength(20)]
  });

  constructor(private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    if (this.groupName) {
      this.currentGroupName = this.groupName;
      this.groupNameForm.get('name')?.setValue(this.groupName.name);
    }

    this.groupNameForm.valueChanges.forEach(() => {
      this.groupNameValid.emit(this.groupNameForm.valid);
      this.currentGroupName = {
        name: this.groupNameFormField!.value ?? ""
      };
      this.groupNameSubmit.emit(this.currentGroupName);
    })
  }

  get groupNameFormField() {
    return this.groupNameForm.get('name');
  }
}

export interface GroupName {
  name: string;
}
