import os
import zipfile
from PIL import Image

Image.MAX_IMAGE_PIXELS = None  # Allow processing large images

ZIP_INPUT = "Missing Assets.zip"
EXTRACT_DIR = "extracted_assets"
OUTPUT_DIR = "resized_assets"
OUTPUT_ZIP = "Resized_Missing_Assets.zip"

print(f"📦 Extracting {ZIP_INPUT}...")
with zipfile.ZipFile(ZIP_INPUT, 'r') as zip_ref:
    zip_ref.extractall(EXTRACT_DIR)
print("✅ Extraction complete.\n")

# Step 2: Resize images
image_count = 0
resized_count = 0

print("🔍 Scanning and processing images...\n")
for root, _, files in os.walk(EXTRACT_DIR):
    for file in files:
        if file.lower().endswith(".png") and not file.endswith(".meta"):
            image_count += 1
            src = os.path.join(root, file)
            rel = os.path.relpath(src, EXTRACT_DIR)
            dst = os.path.join(OUTPUT_DIR, rel)
            os.makedirs(os.path.dirname(dst), exist_ok=True)

            with Image.open(src) as img:
                w, h = img.size
                if w > 2000 or h > 2000:
                    print(f"📉 Resizing: {rel} ({w}x{h})")
                    img = img.resize((max(1, int(w * 0.05)), max(1, int(h * 0.05))), Image.Resampling.LANCZOS)
                    resized_count += 1
                else:
                    print(f"📎 Copying:  {rel} ({w}x{h})")
                img.save(dst)

print(f"\n✅ Processed {image_count} images. Resized {resized_count} of them.\n")

# Step 3: Zip the resized assets
print(f"📦 Creating zip: {OUTPUT_ZIP}...")
with zipfile.ZipFile(OUTPUT_ZIP, 'w', zipfile.ZIP_DEFLATED) as zipf:
    for root, _, files in os.walk(OUTPUT_DIR):
        for file in files:
            full_path = os.path.join(root, file)
            arcname = os.path.relpath(full_path, OUTPUT_DIR)
            zipf.write(full_path, arcname)

print(f"✅ Done: Resized and zipped to '{OUTPUT_ZIP}'")
