import "./App.css";
import ListPerson from "./components/ListPerson";
import ListPersonTypes from "./components/ListPersonTypes";
import { useEffect, useState } from "react";
import type { Person, PersonType } from "./models/model";

function App() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [personTypes, setPersonTypes] = useState<PersonType[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

  const refreshPersonTypes = async () => {
    try {
      const res = await fetch(`${apiBaseUrl}/api/v1/persontypes`);
      if (!res.ok) throw new Error("Failed to fetch person types");
      const data: PersonType[] = await res.json();
      setPersonTypes(data);
    } catch (err: any) {
      setError(err.message || "Failed to refresh person types");
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [personsRes, typesRes] = await Promise.all([
          fetch(`${apiBaseUrl}/api/v1/persons`),
          fetch(`${apiBaseUrl}/api/v1/persontypes`),
        ]);

        if (!personsRes.ok)
          throw new Error(`Failed to fetch persons: ${personsRes.statusText}`);
        if (!typesRes.ok)
          throw new Error(
            `Failed to fetch person types: ${typesRes.statusText}`
          );

        const personsData: Person[] = await personsRes.json();
        const personTypesData: PersonType[] = await typesRes.json();

        setPersons(personsData);
        setPersonTypes(personTypesData);
        setLoading(false);
      } catch (err: any) {
        setError(err.message || "Something went wrong");
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) return <div className="p-4">Loading...</div>;
  if (error) return <div className="p-4 text-danger">Error: {error}</div>;

  return (
    <div className="container mt-4">
      <div className="scroll-section mb-4">
        <ListPerson personTypes={personTypes} initialPersons={persons} />
      </div>
      <div className="scroll-section">
        <ListPersonTypes
          personTypes={personTypes}
          onPersonTypeAdded={refreshPersonTypes}
        />
      </div>
    </div>
  );
}

export default App;
