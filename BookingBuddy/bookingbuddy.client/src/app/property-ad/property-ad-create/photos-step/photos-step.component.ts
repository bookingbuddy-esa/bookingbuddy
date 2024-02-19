import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-photos-step',
  templateUrl: './photos-step.component.html',
  styleUrl: './photos-step.component.css'
})
export class PhotosStepComponent implements OnInit {

  @Input() photos: File[] = [];
  @Output() photosValid = new EventEmitter<boolean>();
  @Output() photosSubmit = new EventEmitter<File[]>();
  protected errors: string[] = [];
  protected selectedFiles: File[] = [];
  protected selectedFilesURL: string[] = [];
  protected readonly maxFiles: number = 5;

  ngOnInit(): void {
    this.selectedFiles = this.photos;
    this.selectedFiles.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.selectedFilesURL.push(e.target!.result as string);
      };
      reader.readAsDataURL(file);
    });
    this.photosValid.emit(this.selectedFiles.length > 0);
  }

  onImagesSelect(event: Event): void {
    this.selectedFiles = [];
    this.selectedFilesURL = [];
    const target = event.target as HTMLInputElement;
    if (target.files) {
      for (let i = 0; i < target.files.length; i++) {
        if (this.selectedFiles.length >= this.maxFiles) {
          this.errors.push('O número máximo de imagens foi atingido.');
          break;
        }
        if (target.files[i].type === 'image/jpeg' || target.files[i].type === 'image/png') {
          this.selectedFiles.push(target.files[i]);
          const reader = new FileReader();
          reader.onload = (e) => {
            this.selectedFilesURL.push(e.target!.result as string);
          };
          reader.readAsDataURL(target.files[i]);
        } else {
          this.errors.push(`${target.files[i].name} não é um ficheiro de imagem válido.`);
        }
      }
    }
    this.photosValid.emit(this.selectedFiles.length > 0);
    this.photosSubmit.emit(this.selectedFiles);
  }

  removeImage(image: string) {
    const index = this.selectedFilesURL.indexOf(image);
    this.selectedFilesURL.splice(index, 1);
    this.selectedFiles.splice(index, 1);
    this.photosValid.emit(this.selectedFiles.length > 0);
  }
}
