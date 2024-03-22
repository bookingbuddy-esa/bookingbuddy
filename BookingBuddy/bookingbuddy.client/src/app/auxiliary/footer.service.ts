import {Injectable} from '@angular/core';
import {BehaviorSubject, Subject} from "rxjs";
import {b} from "@fullcalendar/core/internal-common";

@Injectable({
  providedIn: 'root'
})
export class FooterService {

  private _isFooterVisible: Subject<boolean> = new BehaviorSubject<boolean>(true);

  constructor() {
  }

  public isFooterVisible() {
    return this._isFooterVisible.asObservable();
  }

  public showFooter() {
    this._isFooterVisible.next(true);
  }

  public hideFooter() {
    this._isFooterVisible.next(false);
  }
}
