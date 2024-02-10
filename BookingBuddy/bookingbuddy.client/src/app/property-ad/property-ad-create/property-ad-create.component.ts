import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from '@angular/router';

@Component({
  selector: 'app-property-ad-create',
  templateUrl: './property-ad-create.component.html',
  styleUrl: './property-ad-create.component.css'
})
export class PropertyAdCreateComponent {
  errors: string[] = [];
  createPropertyAdForm!: FormGroup;


  /**
   * Construtor da classe SignInComponent.
   * @param formBuilder Construtor de formulários do Angular.
   * @param router Router do Angular.
   */
  constructor( private formBuilder: FormBuilder, private router: Router) {
    
  }
  

  public create(_: any) {
  
  }
}
