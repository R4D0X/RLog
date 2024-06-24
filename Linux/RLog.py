import os

import logging

import argparse



def loglamayi_ayarla():

    logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')



def dosya_ismi_temizle(url):

    for char in ['://', '/', ':', '*', '?', '"', '<', '>', '|']:

        url = url.replace(char, '')

    return url



def linkleri_filtrele(girdi_dosya_yollari, cikti_dosya_yolu, url_listesi):

    try:

        with open(cikti_dosya_yolu, 'w', encoding='utf-8') as cikti_dosya:

            # İlk satıra "Radox" yaz ve bir boş satır ekle

            cikti_dosya.write("--- Made by Radox ---\n\n")



            for girdi_dosya_yolu in girdi_dosya_yollari:

                girdi_dosya_ismi = os.path.basename(girdi_dosya_yolu)



                try:

                    with open(girdi_dosya_yolu, 'r', encoding='utf-8', errors='ignore') as girdi_dosya:

                        for satir in girdi_dosya:

                            if any(url in satir for url in url_listesi):

                                cikti_dosya.write(f"{girdi_dosya_ismi}: {satir}")

                except Exception as e:

                    logging.error(f"{girdi_dosya_yolu} dosyasını okurken hata: {e}")



    except Exception as e:

        logging.error(f"{cikti_dosya_yolu} dosyasına yazarken hata: {e}")



def main():

    loglamayi_ayarla()



    parser = argparse.ArgumentParser(description="Belirli URL'leri içeren satırları birleştirip tek bir çıktı dosyasına yazan program.")

    parser.add_argument('girdi_dosyalari', nargs='+', help="Girdi dosya yolları")

    parser.add_argument('cikti_dosya', help="Çıktı dosya yolu")

    parser.add_argument('url_listesi', help="Aranacak URL'lerin virgülle ayrılmış listesi")



    args = parser.parse_args()



    girdi_dosya_yollari = args.girdi_dosyalari

    cikti_dosya_yolu = args.cikti_dosya

    url_listesi = [url.strip() for url in args.url_listesi.split(',')]



    linkleri_filtrele(girdi_dosya_yollari, cikti_dosya_yolu, url_listesi)

    logging.info("İşlem tamamlandı.")



if __name__ == "__main__":

    main()

