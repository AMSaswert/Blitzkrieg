
export class Comment{

    Id : number;
    TopicId : number;
    AuthorUsername : string;
    CreationDate : Date;
    ParentCommentId : number;
    ChildrenComments : Array<Comment>;
    Text : string;
    LikesNo : number;
    DislikesNo : number;
    Edited : boolean;
    Removed : boolean;
    UsersWhoVoted : Array<string>;

    constructor(){}

}