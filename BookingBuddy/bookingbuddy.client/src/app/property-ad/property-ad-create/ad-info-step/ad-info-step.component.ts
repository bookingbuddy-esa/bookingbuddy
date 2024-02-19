import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";

@Component({
  selector: 'app-ad-info-step',
  templateUrl: './ad-info-step.component.html',
  styleUrl: './ad-info-step.component.css'
})
export class AdInfoStepComponent implements OnInit {
  @Input() errors: string[] = [];
  @Input() adInfo: AdInfo | undefined;
  @Output() adInfoValid = new EventEmitter<boolean>();
  @Output() adInfoSubmit = new EventEmitter<AdInfo>();
  protected currentAdInfo: AdInfo | undefined;
  protected infoForm = this.formBuilder.group({
    name: ['', Validators.required],
    description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
    pricePerNight: ['', [Validators.required, Validators.min(1)]]
  });

  constructor(private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    if (this.adInfo) {
      this.currentAdInfo = this.adInfo;
      this.infoForm.get('name')?.setValue(this.adInfo.name);
      this.infoForm.get('description')?.setValue(this.adInfo.description);
      this.infoForm.get('pricePerNight')?.setValue(this.adInfo.pricePerNight.toString());
    }

    this.infoForm.valueChanges.forEach(() => {
      this.adInfoValid.emit(this.infoForm.valid);
      this.currentAdInfo = {
        name: this.nameFormField!.value ?? "",
        description: this.descriptionFormField!.value ?? "",
        pricePerNight: parseFloat(this.pricePerNightFormField!.value?? "0")
      };
      this.adInfoSubmit.emit(this.currentAdInfo);
    })
  }

  get nameFormField() {
    return this.infoForm.get('name');
  }

  get descriptionFormField() {
    return this.infoForm.get('description');
  }

  get pricePerNightFormField() {
    return this.infoForm.get('pricePerNight');
  }
}

export interface AdInfo {
  name: string;
  description: string;
  pricePerNight: number;
}
