export enum EntityType
{
    Subforum = 0,
    Topic = 1,
    Comment = 2,
}



export class Complaint{

    Id: number;
    Text : string;
    CreationDate : Date;
    EntityType : EntityType;
    EntityId : number;
    AuthorUsername : string;

    constructor()
    {
    }

}