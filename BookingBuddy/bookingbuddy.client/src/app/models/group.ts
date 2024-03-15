import { Property } from "./property";

export interface Group {
  groupId: string,
  groupOwnerId: string,
  members: string[],
  name: string,
  properties: Property[],
  choosenProperty: string
}
