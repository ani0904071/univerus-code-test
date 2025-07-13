import type { Person, PersonCreate } from "../models/model";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

class PersonService {
  async create(data: PersonCreate): Promise<Person | null> {
    let createdPerson: Person | null = null;
    try {
      const res = await fetch(`${apiBaseUrl}/api/v1/persons`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });

      if (!res.ok) {
        const errorText = await res.text();
        throw new Error(
          `Failed to create person: ${res.status} - ${errorText}`
        );
      }

      createdPerson = await res.json();
    } catch (error) {
      console.error("Error creating person:", error);
      throw error;
    }
    return createdPerson;
  }

  async getPersonById(id: number): Promise<Person> {
    let person: Person | null = null;
    try {
      const res = await fetch(`${apiBaseUrl}/api/v1/persons/${id}`);
      if (!res.ok) {
        const errorText = await res.text();
        throw new Error(
          `Failed to fetch person with ID ${id}: ${res.status} - ${errorText}`
        );
      }

      person = await res.json();
    } catch (error) {
      console.error(`Error fetching person with ID ${id}:`, error);
      throw error;
    }
    if (person === null) throw new Error("Person not found");
    return person;
  }

  async getAllPersons(): Promise<Person[]> {
    let persons: Person[] = [];
    try {
      const res = await fetch(`${apiBaseUrl}/api/v1/persons`);
      if (!res.ok) {
        const errorText = await res.text();
        throw new Error(
          `Failed to fetch persons: ${res.status} - ${errorText}`
        );
      }

      persons = await res.json();
    } catch (error) {
      console.error("Error fetching persons:", error);
      throw error;
    }
    return persons;
  }

  async update(id: number, data: PersonCreate): Promise<boolean> {
    let success = false;
    try {
      const res = await fetch(`${apiBaseUrl}/api/v1/persons/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, ...data }),
      });

      if (res.status !== 204) {
        const errorText = await res.text();
        throw new Error(
          `Failed to update person: ${res.status} - ${errorText}`
        );
      }

      success = true;
    } catch (error) {
      console.error("Error updating person:", error);
      throw error;
    }
    return success;
  }

  async delete(id: number): Promise<boolean> {
    let success = false;
    try {
      const res = await fetch(`${apiBaseUrl}/api/v1/persons/${id}`, {
        method: "DELETE",
      });

      if (res.status !== 204) {
        const errorText = await res.text();
        throw new Error(
          `Failed to delete person: ${res.status} - ${errorText}`
        );
      }

      success = true;
    } catch (error) {
      console.error("Error deleting person:", error);
      throw error;
    }
    return success;
  }
}

const personServiceInstance = new PersonService();
export default personServiceInstance;
