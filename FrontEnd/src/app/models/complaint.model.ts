export enum EntityType
{
    Subforum = 0,
    Topic = 1,
    Comment = 2,
    Message = 3,
}



export class Complaint{

    Id: number;
    Text : string;
    CreationDate : Date;
    EntityType : EntityType;
    EntityId : number;
    AuthorUsername : string;

    constructor(id:number)
    {
        this.Id = id;
    }

}