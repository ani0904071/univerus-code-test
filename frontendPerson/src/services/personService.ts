import type { Person, PersonCreate } from "../models/model";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

class PersonService {
  async create(data: PersonCreate): Promise<Person | null> {
    const res = await fetch(`${apiBaseUrl}/api/v1/persons`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    if (!res.ok) return null;
    return await res.json();
  }

  async getAllPersons(): Promise<Person[]> {
    const res = await fetch(`${apiBaseUrl}/api/v1/persons`);
    if (!res.ok) throw new Error("Failed to fetch persons");
    return res.json();
  }

  async update(id: number, data: PersonCreate): Promise<boolean> {
    const res = await fetch(`${apiBaseUrl}/api/v1/persons/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ id, ...data }),
    });

    return res.status === 204;
  }

  async delete(id: number): Promise<boolean> {
    const res = await fetch(`${apiBaseUrl}/api/v1/persons/${id}`, {
      method: "DELETE",
    });

    return res.status === 204;
  }
}

const personServiceInstance = new PersonService();
export default personServiceInstance;

//reference: https://medium.com/@bhairabpatra.iitd/api-consumption-best-practices-in-reactjs-using-service-0c3f35299804
