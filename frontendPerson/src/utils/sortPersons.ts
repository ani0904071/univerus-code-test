import type { Person } from "../models/model";

export type SortKey = "name" | "age" | "personTypeId";
export type SortDirection = "asc" | "desc";

/**
 * Returns a sorted copy of the input array of persons.
 */
export function sortPersons(
  persons: Person[],
  sortKey: SortKey,
  sortDirection: SortDirection
): Person[] {
  return [...persons].sort((a, b) => {
    const aValue = a[sortKey];
    const bValue = b[sortKey];

    if (typeof aValue === "string" && typeof bValue === "string") {
      return sortDirection === "asc"
        ? aValue.localeCompare(bValue)
        : bValue.localeCompare(aValue);
    } else {
      return sortDirection === "asc"
        ? (aValue as number) - (bValue as number)
        : (bValue as number) - (aValue as number);
    }
  });
}
