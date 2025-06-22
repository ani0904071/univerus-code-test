import type { PersonType } from '../models/model';

type Props = {
  personTypes: PersonType[];
};

function ListPersonTypes({ personTypes }: Props) {
  return (
    <div className="container mt-5">
      <h2>List of Person Types</h2>
      {personTypes.length === 0 ? (
        <p>No person types found.</p>
      ) : (
        <ul className="list-group">
          {personTypes.map(type => (
            <li key={type.id} className="list-group-item">
              <strong>{type.description}</strong> (ID: {type.id})
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default ListPersonTypes;
