import "./App.css";
import ListPerson from "./components/ListPerson";
import { useEffect, useState } from "react";
import type { Person, PersonType } from "./models/model";

function App() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [personTypes, setPersonTypes] = useState<PersonType[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

  useEffect(() => {
    const fetchPersons = async () => {
      try {
        const response =  await fetch(`${apiBaseUrl}/api/persons`);
        console.log("Fetching persons from:", response.url);
        if (!response.ok) {
          throw new Error(`Failed to fetch persons: ${response.statusText}`);
        }
        const data: Person[] = await response.json();
        setPersons(data);

        // Extract unique personTypes
        const typeMap = new Map<number, PersonType>();
        data.forEach((person) => {
          if (person.personType) {
            typeMap.set(person.personType.id, person.personType);
          }
        });

        setPersonTypes(Array.from(typeMap.values()));
        setLoading(false);
      } catch (err: any) {
        setError(err.message || "Something went wrong");
        setLoading(false);
      }
    };

    fetchPersons();
  }, []);

  if (loading) return <div className="p-4">Loading...</div>;
  if (error) return <div className="p-4 text-danger">Error: {error}</div>;

  return (
    <div>
      <ListPerson personTypes={personTypes} initialPersons={persons} />
    </div>
  );
}

export default App;
