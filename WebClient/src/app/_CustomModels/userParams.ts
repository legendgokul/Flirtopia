import { User } from "./user";

export class userParams{
    constructor(user : User){
        this.gender = user.gender === "male"?"female":"male";
    }

    minAge:number = 18;
    maxAge:number = 99;
    gender:string;
    pageNumber:number = 1;
    pageSize:number = 5;
    orderBy:string = "lastActive";

}