// models.ts

export interface PersonType {
  id: number;
  description: string;
}

export interface Person {
  id: number;
  name: string;
  age: number;
  personTypeId: number;
  personType: PersonType;
}

export interface PersonCreate {
  name: string;
  age: number;
  personTypeId: number;
}