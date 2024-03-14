import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject, interval, map, Observable, of, Subject, timeout } from 'rxjs';
import { GroupService } from '../group.service';
import { AuthorizeService } from '../../auth/authorize.service';
import { UserInfo } from "../../auth/authorize.dto";
import { Group, GroupCreate } from "../../models/group";
import { GroupName } from "./group-name-step/group-name-step.component";
import { GroupLink } from "./group-link-step/group-link-step.component";
import { AppComponent } from '../../app.component';

@Component({
  selector: 'app-group-create',
  templateUrl: './group-create.component.html',
  styleUrl: './group-create.component.css'
})
export class GroupCreateComponent {

}
