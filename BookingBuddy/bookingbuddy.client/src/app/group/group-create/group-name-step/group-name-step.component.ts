import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";

@Component({
  selector: 'app-group-name-step',
  templateUrl: './group-name-step.component.html',
  styleUrl: './group-name-step.component.css'
})
export class GroupNameStepComponent {
  @Input() errors: string[] = [];
  @Input() groupName: GroupName | undefined;
  @Output() groupNameValid = new EventEmitter<boolean>();
  @Output() groupNameSubmit = new EventEmitter<GroupName>();
  protected currentGroupName: GroupName | undefined;
  protected nameForm = this.formBuilder.group({
    name: ['', Validators.required, Validators.minLength(3), Validators.maxLength(20)]
  });

  constructor(private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    if (this.groupName) {
      this.currentGroupName = this.groupName;
      this.nameForm.get('name')?.setValue(this.groupName.name);
    }

    this.nameForm.valueChanges.forEach(() => {
      this.groupNameValid.emit(this.nameForm.valid);
      this.currentGroupName = {
        name: this.nameFormField!.value ?? ""
      };
      this.groupNameSubmit.emit(this.currentGroupName);
    })
  }

  get nameFormField() {
    return this.nameForm.get('name');
  }
}

export interface GroupName {
  name: string;
}
