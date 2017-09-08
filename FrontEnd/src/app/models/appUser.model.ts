import{Message} from './message.model';

export class AppUser{
    Id: number;
    UserName: string;
    Email: string;
    Password: string;
    Role: string;
    Name: string;
    Surname: string;
    ContactPhone: string;
    RegistrationDate: Date;
    BookmarkedSubforums: Array<number>;
    SavedTopics : Array<number>;
    SavedComments : Array<number>;
    ReceivedMessages : Array<Message>;

}