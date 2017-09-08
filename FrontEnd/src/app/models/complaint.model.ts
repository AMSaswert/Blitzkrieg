export enum EntityType
{
    Subforum,
    Topic,
    Comment,
    Message
}



export class Complaint{

    Id: number;
    Text : string;
    CreationDate : Date;
    EntityType : EntityType;
    EntityId : number;
    AuthorUsername : string;

}