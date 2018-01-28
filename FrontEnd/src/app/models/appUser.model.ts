import{Message} from './message.model';
import{Topic} from './topic.model';
import{Comment} from './comment.model';

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
    SavedTopics : Array<Topic>;
    SavedComments : Array<Comment>;
    ReceivedMessages : Array<Message>;

    constructor()
    {
        this.BookmarkedSubforums = [];
        this.SavedTopics = [];
        this.SavedComments = [];
    }

}