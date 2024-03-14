import { ApplicationUser } from "./applicationUser";
import { Property } from "./property";

export interface Group {
  groupId: string;
  groupOwnerId: string;
  groupOwner?: ApplicationUser;
  name: string;
  properties?: Property[];
  members?: string[];
  chosenProperty?: Property;
}

export interface GroupCreate {
  name: string;
  properties: string[];
  members: string[];
}
