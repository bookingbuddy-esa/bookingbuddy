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
  "votes": GroupVote[],
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

export interface GroupVote {
  "userId": string,
  "propertyId": string,
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

export class GroupActionHelper {
  public static getGroupActions(): string[] {
    return Object.keys(GroupAction).filter(k => typeof GroupAction[k as any] === "number")
  }

  public static parseGroupAction(groupAction: string): GroupAction | null {
    switch (groupAction) {
      case "None":
        return GroupAction.none;
      case "Voting":
        return GroupAction.voting;
      case "Booking":
        return GroupAction.booking;
      case "Paying":
        return GroupAction.paying;
      default:
        return null;
    }
  }

  public static AsString(groupAction: GroupAction): string {
    switch (groupAction) {
      case GroupAction.voting:
        return "Voting";
      case GroupAction.booking:
        return "Booking";
      case GroupAction.paying:
        return "Paying";
      default:
        return "None";
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
