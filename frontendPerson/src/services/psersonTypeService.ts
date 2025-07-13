import type { PersonType } from "../models/model";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

class PersonTypeService {
  async getAllPersonTypes(): Promise<PersonType[]> {
    const res = await fetch(`${apiBaseUrl}/api/v1/persontypes`);
    if (!res.ok) throw new Error("Failed to fetch person types");
    return res.json();
  }
}

const personTypeServiceInstance = new PersonTypeService();
export default personTypeServiceInstance;
