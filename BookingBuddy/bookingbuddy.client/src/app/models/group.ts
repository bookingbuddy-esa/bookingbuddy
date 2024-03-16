import { Property } from "./property";

export interface Group {
  groupId: string,
  groupOwnerId: string,
  name: string,
  membersId: string[],
  members: string[],
  propertiesId: string[],
  properties: Property[],
  choosenProperty: string
}

export interface GroupCreate {
  name: string;
  propertyId?: string;
  members?: string[]
}
