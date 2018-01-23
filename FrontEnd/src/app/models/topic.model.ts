import{Comment} from './comment.model';

export enum TopicType{
    Text,
    Picture,
    Link
}


export class Topic{
    Id: number;
    SubforumId : number;
    Name : string;
    TopicType : TopicType;
    AuthorUsername : string;
    Content : string;
    CreationDate : Date;
    LikesNum : number;
    DislikesNum : number;
    Comments : Array<Comment>;
    UsersWhoVoted : Array<string>;

    constructor(){}
}