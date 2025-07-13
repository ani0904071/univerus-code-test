import "./App.css";
import ListPerson from "./components/Person/ListPerson";
import ListPersonTypes from "./components/PersonType/ListPersonTypes";
import { useEffect, useState } from "react";
import type { Person, PersonType } from "./models/model";
import personService from "./services/personService";
import personTypeService from "./services/psersonTypeService";

function App() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [personTypes, setPersonTypes] = useState<PersonType[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const addPersonType = (newType: PersonType) => {
    setPersonTypes((prev) => [...prev, newType]);
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [personsData, personTypesData] = await Promise.all([
          personService.getAllPersons(),
          personTypeService.getAllPersonTypes(),
        ]);

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
          onPersonTypeAdded={addPersonType}
        />
      </div>
    </div>
  );
}

export default App;
