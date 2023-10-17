import { photo } from "./photo"

export interface member {
    id: number
    userName: string
    photoUrl: string
    age: number
    knownAs: string
    createdOn: string
    lastActive: string
    gender: string
    introduction: string
    lookingFor: string
    interests: string
    city: string
    country: string
    photos: photo[]
  }
  
