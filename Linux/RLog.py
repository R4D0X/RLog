# Updated by @liverturk

import tkinter as tk
from tkinter import filedialog, simpledialog
import os
import logging
from concurrent.futures import ThreadPoolExecutor

def setup_logging():
    logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')

def sanitize_filename(url):
    for char in ['://', '/', ':', '*', '?', '"', '<', '>', '|']:
        url = url.replace(char, '_')
    return url

def process_file(input_file_path, urls):
    matching_lines = []
    try:
        with open(input_file_path, 'r', encoding='utf-8', errors='ignore') as input_file:
            for line in input_file:
                for url in urls:
                    if url in line:
                        matching_lines.append(line)
                        break
    except Exception as e:
        logging.error(f"Error occurred: {e}")
    return matching_lines

def filter_links(input_file_paths, output_file_path, urls, max_workers):
    try:
        with open(output_file_path, 'w', encoding='utf-8') as output_file_handle:
            with ThreadPoolExecutor(max_workers=max_workers) as executor:
                futures = [executor.submit(process_file, input_file_path, urls) for input_file_path in input_file_paths]
                for future in futures:
                    matching_lines = future.result()
                    if matching_lines:
                        output_file_handle.writelines(matching_lines)
    except Exception as e:
        logging.error(f"Error occurred: {e}")

def get_user_input():
    root = tk.Tk()
    root.withdraw()

    input_file_paths = filedialog.askopenfilenames(
        title="Select files",
        filetypes=[("Text Files", "*.txt"), ("Backup Text Files", "*.txt~"), ("All Files", "*.*")]
    )
    if not input_file_paths:
        logging.info("Operation canceled")
        return None, None, None, None

    output_folder = filedialog.askdirectory(title="Select the folder to save the output file")
    if not output_folder:
        logging.info("Canceled")
        return None, None, None, None

    urls = simpledialog.askstring("URLs", "Enter the URLs to search separated by commas:")
    if not urls:
        logging.info("No URLs provided canceled")
        return None, None, None, None

    max_workers = os.cpu_count()  
    logging.info(f"Using {max_workers} threads.")

    return input_file_paths, output_folder, [url.strip() for url in urls.split(',')], max_workers

def main():
    setup_logging()
    input_file_paths, output_folder, urls, max_workers = get_user_input()
    if input_file_paths and output_folder and urls:
        output_file_path = os.path.join(output_folder, "filtered_output.txt")
        filter_links(input_file_paths, output_file_path, urls, max_workers)
        logging.info("Operation completed.")

if __name__ == "__main__":
    main()

