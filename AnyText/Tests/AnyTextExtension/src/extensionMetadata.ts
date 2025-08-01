import * as fs from 'fs';
import * as path from 'path';

export interface ContributedLanguage {
  id: string;
  extensions?: string[];
}

export function getContributedLanguages(): ContributedLanguage[] {
  const packageJsonPath = path.resolve(__dirname, '..', 'package.json');
  const raw = fs.readFileSync(packageJsonPath, 'utf-8');
  const pkg = JSON.parse(raw);

  return (pkg.contributes?.languages ?? []) as ContributedLanguage[];
}
