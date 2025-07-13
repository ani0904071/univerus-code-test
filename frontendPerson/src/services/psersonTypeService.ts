import type { PersonType } from "../models/model";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

class PersonTypeService {
  async getAllPersonTypes(): Promise<PersonType[]> {
    let personTypes: PersonType[] = [];
    try {
      const res = await fetch(`${apiBaseUrl}/api/v1/persontypes`);
      if (!res.ok) {
        const errorText = await res.text();
        throw new Error(
          `Failed to fetch person types: ${res.status} - ${errorText}`
        );
      }

      personTypes = await res.json();
    } catch (error) {
      console.error("Error fetching person types:", error);
      throw error;
    }

    return personTypes;
  }
}

const personTypeServiceInstance = new PersonTypeService();
export default personTypeServiceInstance;
