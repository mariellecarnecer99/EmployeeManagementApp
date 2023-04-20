import { Photo } from "./photo"


export interface Member {
    id: number
    userName: string
    photoUrl: string
    age: number
    knownAs: string
    created: string
    lastActive: string
    gender: string
    position: string
    city: string
    country: string
    photos: Photo[]
}
