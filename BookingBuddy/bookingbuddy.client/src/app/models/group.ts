export interface Group {
  "groupId": string,
  "groupBookingId": string | null,
  "groupOwner": {
    "id": string,
    "name": string,
  },
  "name": string,
  "members": GroupMember[],
  "properties": GroupProperty[],
  "chosenProperty": string | null,
  "chatId": string
  "groupAction": string
}

export interface GroupMember {
  "id": string,
  "name": string,
}

export interface GroupProperty {
  "propertyId": string,
  "name": string,
  "pricePerNight": number,
  "imagesUrl": string[],
  "location": string,
  "addedBy": GroupMember,
}

export interface GroupCreate {
  name: string;
  propertyId?: string;
  members?: string[]
}

export enum GroupAction {
  none,
  voting,
  booking,
  paying
}

export class GroupHelper {
  public static getGroupActions(): string[] {
    return Object.keys(GroupAction).filter(k => typeof GroupAction[k as any] === "number")
  }

  public static parseGroupAction(groupAction: string): GroupAction {
    switch (groupAction) {
      case "Voting":
        return GroupAction.voting;
      case "Booking":
        return GroupAction.booking;
      case "Paying":
        return GroupAction.paying;
      default:
        return GroupAction.none;
    }
  }

  public static getGroupActionDisplayName(groupAction: GroupAction): string {
    switch (groupAction) {
      case GroupAction.paying:
        return "A pagar";
      case GroupAction.voting:
        return "A votar";
      case GroupAction.booking:
        return "A reservar";
      default:
        return "Nenhuma ação";
    }
  }
}
