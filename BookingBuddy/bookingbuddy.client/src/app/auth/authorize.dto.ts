// login and register
export interface UserDto {
  email: string;
  password: string;
}

// manage/info
export interface UserInfo {
  name: string;
  userName: string;
  email: string;
  isEmailConfirmed: boolean;
}
