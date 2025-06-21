
import type { Person } from '../models/model';

interface Props {
  person: Person;
  onEdit: () => void;
  onDelete: () => void;
}

function PersonItem({ person, onEdit, onDelete }: Props) {
  return (
    <li className="list-group-item list-group-item-action d-flex justify-content-between align-items-center">
      <div>
        <strong>{person.name}</strong> (Age: {person.age}) - {person.personType.description}
      </div>
      <div>
        <button className="btn btn-primary btn-sm me-2" onClick={onEdit}>View</button>
        <button className="btn btn-danger btn-sm" onClick={onDelete}>Delete</button>
      </div>
    </li>
  );
}

export default PersonItem;
