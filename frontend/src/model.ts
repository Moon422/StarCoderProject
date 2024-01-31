export enum TaskStatus {
    INCOMPLETE,
    COMPLETE
}

export enum ProfileTypes {
    ADMIN, USER
}

export interface ReadTask {
    id: number,
    title: string,
    description: string,
    profileId: number,
    taskStatus: TaskStatus,
}

export interface CreateTask {
    title: string,
    description: string,
    profileId: number,
}

export interface UpdateTask {
    title: string,
    description: string,
    profileId: number,
    taskStatus: TaskStatus,
}

export interface LoginCredentials {
    username: string,
    password: string,
}

export interface LoginResponse {
    profileId: number,
    firstname: string,
    lastname: string,
    email: string,
    profileType: ProfileTypes,
    jwtToken: string,
}

export interface Registration {
    firstname: string,
    lastname: string,
    email: string,
    username: string,
    password: string,
}
