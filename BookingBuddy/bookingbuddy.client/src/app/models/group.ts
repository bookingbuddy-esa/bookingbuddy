import { Property } from "./property";

export interface Group {
  groupId: string,
  groupOwnerId: string,
  name: string,
  membersId: string[],
  members: GroupMember[],
  propertiesId: string[],
  properties: Property[],
  choosenProperty: string,
  messages: GroupMessage[]
}

export interface GroupCreate {
  name: string;
  propertyId?: string;
  members?: string[]
}

export interface GroupMessage {
  messageId?: string,
  userName: string,
  message: string
}

export interface GroupMember {
  name: string,
  id: string
}