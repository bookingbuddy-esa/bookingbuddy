// login and register
export interface UserDto {
  email: string;
  password: string;
}

// manage/info
export interface UserInfo {
  userId: string;
  provider: string;
  roles: string[];
  name: string;
  userName: string;
  email: string;
  isEmailConfirmed: boolean;
  description: string;
}
