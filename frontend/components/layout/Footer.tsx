import { cacheLife } from "next/cache";

async function getCurrent(){
  "use cache";
  cacheLife("max");
  return new Date().getFullYear();
}

export default async function Footer() {
  const year = await getCurrent();
  return (
    <footer className="border-t py-4 text-center text-sm text-muted-foreground">
      © {year} Medora Clinic. All rights reserved.
    </footer>
  );
}