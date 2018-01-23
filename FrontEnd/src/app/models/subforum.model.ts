import{Topic} from './topic.model';



export class Subforum{
    Id : number;
    Name : string;
    Description : string;
    IconURL : string;
    Rules : string;
    LeadModeratorUsername : string;
    Moderators : Array<string>;
    Topics : Array<Topic>;

    constructor() {}

}