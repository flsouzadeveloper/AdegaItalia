using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AdegaItalia.Models;
using Newtonsoft.Json;

namespace AdegaItalia.Controllers
{
    public class EcommerceController : Controller
    {

        CarrinhoModel carrinho = new CarrinhoModel();
        List<CarrinhoModel> listaCarrinho = new List<CarrinhoModel>();

        public ActionResult Ecommerce()
        {
            try
            {
                TempData["carrinho"] = new List<CarrinhoModel>();
                
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult _Ecommerce()
        {
            if (TempData["carrinho"] == null)
                TempData["carrinho"] = new List<CarrinhoModel>();

            var model = new EcommerceModel();
            model.token = GerarToken();
            model.listaProdutos = CarregarArquivos();

            return PartialView("_Ecommerce", model);
        }

        [HttpPost]
        public ActionResult AddCarrinho(EcommerceModel model)
        {
            if (model.quantidade <= 0 || model.quantidade == null)
            {
                model.quantidade = 1;
            }
            if (TempData["carrinho"] != null)
                listaCarrinho = TempData["carrinho"] as List<CarrinhoModel>;

            List<ProdutosModel> lista = new List<ProdutosModel>();
            lista = CarregarArquivos();

            carrinho.itemId = (from p in lista where p.id == model.id select p.id).First();
            carrinho.itemQuantity = model.quantidade;
            carrinho.itemDescription = (from p in lista where p.id == model.id select p.descricao).First();
            carrinho.itemAmount = (from p in lista where p.id == model.id select p.valor.ToString()).First();

            listaCarrinho.Add(carrinho);
            TempData["carrinho"] = listaCarrinho;
            TempData.Keep("carrinho");
            EcommerceModel ecommerce = new EcommerceModel();
            ecommerce = model;
            ecommerce.token = GerarToken();
            ecommerce.listaProdutos = lista;
            ecommerce.listaProdutosCarrinho = listaCarrinho;
            return PartialView("_Ecommerce", model);
        }

        public ActionResult FinalizarCompra(string a)
        {
            List<CarrinhoModel> listaCarrinho = new List<CarrinhoModel>();
            listaCarrinho = TempData["carrinho"] as List<CarrinhoModel>;

            string token = "";
            string conteudo = "";
            int acum = 1;
            foreach (var l in listaCarrinho)
            {
                l.itemQuantity = (from p in listaCarrinho where p.itemId == l.itemId select p.itemQuantity).First();
                l.itemDescription = (from p in listaCarrinho where p.itemId == l.itemId select p.itemDescription).First();
                l.itemAmount = (from p in listaCarrinho where p.itemId == l.itemId select p.itemAmount.ToString()).First();

                conteudo += String.Format(@"&itemId{4}={0}&itemQuantity{4}={1}&itemDescription{4}={2}&itemAmount{4}={3}", l.itemId, l.itemQuantity, l.itemDescription, l.itemAmount, acum);
                acum++;
            }

            string data = "email=vinhosadegaditalia@hotmail.com&token=DD737308D09B492985570E10F89D8FED&currency=BRL" + conteudo;

            HttpWebRequest request = GenerateRequest("https://ws.pagseguro.uol.com.br/v2/checkout/", data, "POST");

            string tokenCompleto = GetResponseContent(GetResponse(request));
            int inicioRetorno = tokenCompleto.IndexOf("<code>") + 6;
            int fimRetorno = tokenCompleto.IndexOf("</code>");
            token = tokenCompleto.Substring(inicioRetorno, fimRetorno - inicioRetorno);

            EcommerceModel ecommerce = new EcommerceModel();
            ecommerce.token = token;
            ecommerce.listaProdutos = CarregarArquivos();
            return Json(token, JsonRequestBehavior.AllowGet);
        }

        public string GerarToken(){

            string token = "";
            string data = "email=vinhosadegaditalia@hotmail.com&token=DD737308D09B492985570E10F89D8FED";

            HttpWebRequest request = GenerateRequest("https://ws.pagseguro.uol.com.br/v2/sessions/", data, "POST");

            string tokenCompleto = GetResponseContent(GetResponse(request));
            int inicio = tokenCompleto.IndexOf("<id>") + 4;
            int fim = tokenCompleto.IndexOf("</id>");
            token = tokenCompleto.Substring(inicio, fim - inicio);
            return token;
        }

        public List<ProdutosModel> CarregarArquivos()
        {
            var lista = new List<ProdutosModel>();
            var produto = new ProdutosModel();

            lista.Add(new ProdutosModel() { disponivel = true, valor = "93.00", caminhoImage = "~/Images/VinhosEcommerce/Refosco Penduculo Rosso.png", descricao = "Refosco Penduculo Rosso", id = 1, detalhe = "Uva: 100% refosco. Cor vermelho rubi. No olfato reúne um intenso perfume. É um Vinho frutado sabor de amora madura. Gosto importante, fascinante, decisivo e harmonioso com grande equilíbrio e prazer.Vol. 12,50 - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "85.00", caminhoImage = "~/Images/VinhosEcommerce/Merlot.png", descricao = "Merlot", id = 2, detalhe = "Uva: Merlot 100%. Nota: Cor vermelho rubi. Aroma amadeirado, doce e delicado com nota de baunilha, é uma combinação perfeita. Sabor: É um vinho sugestivo, simples e não é empenhativo, mas é complexo e importante, aveludado e quente com tainho controlada. Estrutura que acompanha seu envelhecimento. Vol:13,5% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "150.00", caminhoImage = "~/Images/VinhosEcommerce/Merloc Doc Piave Moletto.png", descricao = "Merloc Doc Piave Moletto", id = 3, detalhe = "Uva: Merlot 100%. No olfato sente-se o aroma de fruta madura (fruta vermelha). Elegante na boca, com tanino doce. Um vinho reconhecível, com bom corpo, que satisfaz o paladar. Para ser apreciado com carne e massas. Vol: 13,5 - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "400.00", caminhoImage = "~/Images/VinhosEcommerce/Cabernet Sauvignon Select Reserva.png", descricao = "Cabernet Sauvignon Select Reserva", id = 4, detalhe = "Uva: Cabernet sauvignon 100%. É um grande vinho, com 6 anos de maturação em barril, tendo um grande equilíbrio. Nota: Cor vermelho rubi. Bouquet complexo cacau, couro, chocolate, possibilitando sentir o aroma de fruta vermelha, com cereja. No paladar é um vinho de grande equilíbrio. Tanino e ácido equilibrado. Há percepções gustativas também no olfato além de um leve tom picante. Definitivamente é um vinho que vai ter o que contar com o tempo. Vol: 14,50% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "75.00", caminhoImage = "~/Images/VinhosEcommerce/Cabernet Moletto.png", descricao = "Cabernet Moletto", id = 5, detalhe = "Uva: Cabernet. Nota: Cor vermelho rubi brilhante. Bouquet: Rico e complexo. Intenso elegantíssimo. Sabor de ervas (rúcula, pimenta do reino). Vol: 12,5 - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "320.00", caminhoImage = "~/Images/VinhosEcommerce/Colmello Rosso Igt Veneto Oriental Moletto.png", descricao = "Colmello Rosso Igt Veneto Oriental Moletto", id = 6, detalhe = "Uva: Merlot 60%, fraconia 40%. Nota: Cor vermelho rubi, depois de sete anos envelhecido em barril da origem a sua cor. Perfume delicado, percebe-se já no aroma o perfeito equilíbrio entre vinho e madeira, harmonização perfeita, fruta vermelha, baunilha, com aroma mais complexo chocolate, cacau, couro, noz moscada e pimenta do reino. Selecionado de uvas merlot e fraconia, que se dá vida a esse vinho tinto elegante vinhoso, aromas equilibrados com ótima estrutura no olfato, e complexo sabor quente e aveludado, sua suavidade é tal que emite muitas combinações diferentes. Vol: 14,50 - Prodotto in Italaia" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "900.00", caminhoImage = "~/Images/VinhosEcommerce/Colmello Rosso Igt Veneto Oriental Magmum.png", descricao = "Colmello Rosso Igt Veneto Oriental Magmum", id = 7, detalhe = "Uva: Merlot e fraconia. Nota: Cor vermelho rubi, depois de sete anos envelhecido em barril da origem a sua cor. Perfume delicado percebe-se já no aroma o perfeito equilíbrio entre vinho e madeira, harmonização perfeita, fruta vermelha, baunilha, com aroma mais complexo chocolate, cacau, couro, noz moscada e pimenta do reino. Selecionado de uvas merlot e fraconia, que se dá vida a esse vinho tinto elegante vinhoso, aromas equilibrados com ótima estrutura no olfato, e complexo sabor quente e aveludado, sua suavidade é tal que emite muitas combinações diferentes. Vol: 14,50" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "75.00", caminhoImage = "~/Images/VinhosEcommerce/Pinot Bianco Moletto Igt.png", descricao = "Pinot Bianco Moletto Igt", id = 8, detalhe = "Uva: Pinot bianco 100%. É um vinho branco de grande fineza. A colheita em 2013 foi surpreendido por oferecer um vinho frutado particularmente com gosto de maçã verde. Ótimo como aperitivos leves, massas à base de legumes, peixe e pratos delicados. Vol: 12% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "75.00", caminhoImage = "~/Images/VinhosEcommerce/Pinot Grigio Moletto Doc Venezia.png", descricao = "Pinot Grigio Moletto Doc Venezia", id = 9, detalhe = "Uva: Pinot grigio 100%. Delicadamente frutado e floral. Um vinho branco e caráter decidido! Particularmente adequado para emparelhar com pratos à base de peixe, ou como aperitivo. Vol: 12% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "80.00", caminhoImage = "~/Images/VinhosEcommerce/Sauvignon Moletto Igt Veneto Oriental.png", descricao = "Sauvignon Moletto Igt Veneto Oriental", id = 10, detalhe = "Uva: Sauvignon 100%. Uma explosão de aromas, vinho branco fino e de grande personalidade, com corpo equilibrado aveludado e redondo. Ótimo para jantares e ser consumido no dia a dia. Vol: 12,5% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "85.00", caminhoImage = "~/Images/VinhosEcommerce/Chardonnay Moleto Doc Lison Pramaggiore.png", descricao = "Chardonnay Moleto Doc Lison Pramaggiore", id = 11, detalhe = "Uva: Chardonay 100%. E um vinho branco agradável, equilibrado com bouquet elegante, com nota frutada que satisfaz o palato. É perfeito para acompanhamento com entrada italiana. Perfume de maçã verde com uma delicada nota doce. Vol: 12% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "93.00", caminhoImage = "~/Images/VinhosEcommerce/Espumante Glisy Igt Veneto Oriental.png", descricao = "Espumante Glisy Igt Veneto Oriental", id = 12, detalhe = "Uva: Glera Chardonay. Aromática, atraente, meio-corpo. Um vinho espumante fácil de beber e com baixo teor alcoólico. Perfeito como aperitivo ideal para grandes eventos, mas também pode acompanhar qualquer tipo de refeição. Vol: 10% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "120.00", caminhoImage = "~/Images/VinhosEcommerce/Proseco Treviso Doc Extra Dry.png", descricao = "Proseco Treviso Doc Extra Dry", id = 13, detalhe = "Uva: Glera 100%. Com perlagge versátil é sempre agradável aroma frutado. Elegante e fresco, é um espumante para toda ocasião, sabor fresco frutado e elegante, corpo harmônico acompanhado de borbulhas aveludadas. Vol: 11% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "140.00", caminhoImage = "~/Images/VinhosEcommerce/Brut Millessimato Vsq.png", descricao = "Brut Millessimato Vsq", id = 14, detalhe = "Uva: Pinot Bianco 100%, metade charmat longo (24 meses em autoclave), insolito para uvas Pinot bianco, longa permanência em autoclave. É um processo incrivelmente frutado e corposo de extrema elegância, perfeito para brindar em momentos especiais. De cor amarelo vivo brilhante com perlage finissíma e brilhante, de aroma surpreendente: no olfato e particularmente frutado e seco. De borbulha decisivamente fina e elegante. Vol: 13% - Ml. 750 - Ml. - 1.5l" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "95.00", caminhoImage = "~/Images/VinhosEcommerce/Chianti Colli Senesi Docg.png", descricao = "Chianti Colli Senesi Docg", id = 15, detalhe = "Denominação de Origem controlada. Uva: Sangiovese 90%, Canaiola nero 10%. Envelhecimento: um curto período (3 meses no máximo), em barris de carvalho eslavo, seguido por alguns meses em garrafa. País de origem: Itália. Vol: 13,50% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "115.00", caminhoImage = "~/Images/VinhosEcommerce/Rosso Di Montepulciano Doc.png", descricao = "Rosso Di Montepulciano Doc", id = 16, detalhe = "Denominação de Origem controlada. Uva: Prugnolo Gentile (Sangiovese) 90%, Mammolo 10%. Envelhecimento: 9 meses em barricas de carvalho francês e em barris de carvalho eslavo, seguido por 3 meses em garrafa. Vol: 13,00% - Ml. 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "180.00", caminhoImage = "~/Images/VinhosEcommerce/Vino Nobile di Montepulciano.png", descricao = "Vino Nobile di Montepulciano", id = 17, detalhe = "Denominação de Origem controlada e garantida. Envelhecimento: 9 meses em barricas de carvalho eslavo, seguidos por 3 meses em garrafa. País de origem: Itália. Vol: 13,50% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "380.00", caminhoImage = "~/Images/VinhosEcommerce/Brunello Di Montalcino Doc.png", descricao = "Brunello Di Montalcino Doc", id = 18, detalhe = "Uva: Sangiovese 100%. Pelo seu grande longo envelhecimento ganha cor vermelha, alta expressão da tradição de Montalcino. De uma cor rubi intenso com reflexo granada e perfumes concentrado, envolventem, etéreo, com notas de cereja e baunilha. O sabor é decisivo, quando jovem o tanino e suave e persistente. Envelhecimento: Depois de um período em autoclave de aço: passa-se 60% em grandes barris de carvalho eslavo e 40% em barris nos primeiros 12 meses. E mais 18 meses final em garrafa. Vol 14% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "380.00", caminhoImage = "~/Images/VinhosEcommerce/Poggio Abate Brunello Montalcino Reserva Docg.png", descricao = "Poggio Abate Brunello Montalcino Reserva Docg", id = 19, detalhe = "Uva: Sangiovese 100%. E uma característica única e especial, o resultado das mais belas vinhas da toscana. A cor vermelha rubi intensa, promete o aroma de cereja preta e frutas vermelhas. Na boca é ainda mais complexo pelo sabor do baunilha e tabaco. Longo e persistente, este Brunello presta homenagem ao seu território a sua vocação como vinho de excelência. Envelhecimento: Após 6 meses em autoclave de aço inox, 15 meses em barricas de carvalho francês, seguidos por mais 15 meses em grandes barris de carvalho da Eslovênia. Para depois ser envelhecida durante 24 meses em garrafa. Vol: 14,5% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "130.00", caminhoImage = "~/Images/VinhosEcommerce/Vermentino Di Toscana.png", descricao = "Vermentino Di Toscana", id = 20, detalhe = "Denominação de Origem controlada. Uva: Vermentino 100%. Envelhecimento 2 a 4 anos em garrafa. Vol: 12,5% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = false, valor = "70.00", caminhoImage = "~/Images/VinhosEcommerce/Vinho Tinto Castelli Romani.png", descricao = "Vinho Tinto Castelli Romani", id = 21, detalhe = "Uva: Sangiovese, Cesanese, Merlot e Montepulciano. Cor: vermelho rubi com tons violeta. Fragrância: floral e intenso. Gosto: macio, harmonioso e persistente. Temperatura de serviço: Cerca de 16°C. Tradicional. Procedência: da área de origem do Castelli Romani, áreas montanhosas expostas ao sul-oeste de Roma entre 180 e 380 metros acima do nível do mar. Vol: 12,5% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = false, valor = "70.00", caminhoImage = "~/Images/VinhosEcommerce/Vinho Tinto Frizante Merlot.png", descricao = "Vinho Tinto Frisante Merlot", id = 22, detalhe = "Uva: Merlot. Cor vermelha com reflexo violeta. Aroma frutado e floreal. No paladar e fresco com boa persistência. Temperatura de serviço: cerca de 10°C. Região: Lazio, nas serras romanas. Vol: 10,5% + Vol:1,5% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = false, valor = "70.00", caminhoImage = "~/Images/VinhosEcommerce/Vinho Rose Castelli Romani.png", descricao = "Vinho Rose Castelli Romani", id = 23, detalhe = "Uva: Sangiovese, Cesanese, Ciliegiolo álcool. Cor: rosa brilhante. Aroma frutado e fresco ao paladar, doce, macio e aveludado. Servir temperatura: 10°C. Vinho rosé Castelli Romani. Região: Lazio. Área de origem protegida do Castelli Romani. Vol: 11% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = false, valor = "60.00", caminhoImage = "~/Images/VinhosEcommerce/Vinho Branco Frascati.png", descricao = "Vinho Branco Frascati", id = 24, detalhe = "Uva: Malvasia di Candia e Lazio, Trebbiano Toscano e grego. Cor: amarelo. Bouquet: frutado fresco. Sabor: delicado, harmonioso e aromático. Temperatura de serviço: cerca de 10°C. Vinho branco Frascati Dop seco. O tradicional vinho branco. Procedência: Região Lazio, área de origem protegida de Frascati, mais ao norte do Castelli Romani, com altitude entre 300 e 400 metros acima do nível do mar, com clima frio e úmido suficientes, ideal para plantação de vinhas. Vol: 12% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "250.00", caminhoImage = "~/Images/VinhosEcommerce/Amarone Valpolicella Classico.png", descricao = "Amarone Valpolicella Classico", id = 25, detalhe = "Safra 2009. Uva: Corvina 60%, Corvinone 20%, Rondinella 15%, Molinara 5%. Região: Veneto valpocella corrubbio (VR). Envelhecimento: 12 meses em barril francês, mais 12 meses em garrafas. Aroma intenso de fruta madura, tipo cereja. Sabor: redondo e aveludado, frutado corposo, com nota Floreal. Vol: 16% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "300.00", caminhoImage = "~/Images/VinhosEcommerce/Barolo.png", descricao = "Barolo", id = 26, detalhe = "Safra 2006. Uva: Nebbiolo 100%. Região: Piemonte. Perfume de espécie violeta cacau e trufas. Sabor: adocicado finamente, deixa na boca um gosto corposo e imponente. Vol: 14% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "300.00", caminhoImage = "~/Images/VinhosEcommerce/Valpolicella Superiore.png", descricao = "Valpolicella Superiore", id = 26, detalhe = "Safra 2011. Uva: Corvina 75%, Rondinella 20%, Molinara 5%. Região: Veneto. Cor: vermelho rubi, aroma de frutas vermelhas, tipo amora, com leve nota de madeira doce, espécie oriental. Vol: 13,50% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "150.00", caminhoImage = "~/Images/VinhosEcommerce/Chianti Classico.png", descricao = "Chianti Classico", id = 28, detalhe = "Reserva 2009. Uva: 100% sangiovese. Região: Toscana. Cor: Notável brilhante, aroma de frutas vermelhas, cereja, amora. Vol: 14% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "190.00", caminhoImage = "~/Images/VinhosEcommerce/Vermentino Di Galurra Dcog.png", descricao = "Vermentino Di Galurra Dcog", id = 29, detalhe = "Uva: Vermentino 100%. Safra 2013. Região: Sardegna. Aroma de flores do campo, gosto seco e aveludado. Vol: 13,50% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "120.00", caminhoImage = "~/Images/VinhosEcommerce/Suave.png", descricao = "Suave", id = 30, detalhe = "Suave DOCG. Safra 2013. Uva: Garganega 70%, Trebbiano di Soave 30%. Região: Veneto soave (VR). Aroma: floral, sabor frutado, que no paladar se torna longamente aveludado e satisfatório. Vol: 12% - Ml 750" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "300.00", caminhoImage = "~/Images/VinhosEcommerce/Aqua Vitae.png", descricao = "Aqua Vitae", id = 31, detalhe = "Spirito D'uva. Uva: prosecco, reisling, verdiso. Região: Veneto. Conegliano (TV). Perfume intenso de uva colhida. Nota fina e elegante de fruta cítrica, uva passa, gosto harmônico, agradavelmente doce ácido. Vol: 21% - Ml 700" });
            lista.Add(new ProdutosModel() { disponivel = true, valor = "300.00", caminhoImage = "~/Images/VinhosEcommerce/Brut Valdobiadene.png", descricao = "Brut Valdobiadene", id = 32, detalhe = "Uva: Glera 100% (prosecco). Região: Veneto Valdobbiadene. É elegante e aromático, gosto frutado. Valdobbiadene Prosecco Superiore DOCG Brut. Vol 11,5% - Ml 750" });

            return lista;
        }

        public ActionResult Retorno()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Retorno(FormCollection colecao)
        {
            return View();
        }

        public ActionResult displayPHPPage(string url)
        {
            using (var client = new WebClient())
            {
                string response = client.DownloadString(url);
                return View(response);
            }
        }

        string GetResponseContent(HttpWebResponse response)
        {
            if (response == null)
            {

                return "FALHA";
            }
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;

            try
            {
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Cleanup the streams and the response.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                response.Close();
            }
            return responseFromServer;
        }


        CookieContainer cookies = new CookieContainer();

        internal string GetCookieValue(Uri SiteUri, string name)
        {
            Cookie cookie = cookies.GetCookies(SiteUri)[name];
            return (cookie == null) ? null : cookie.Value;
        }

        HttpWebRequest GenerateRequest(string uri, string content, string method)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            // Set the Method property of the request to POST.
            request.Method = method;
            // Set cookie container to maintain cookies
            request.CookieContainer = cookies;
            //request.AllowAutoRedirect = allowAutoRedirect;

            // If login is empty use defaul credentials
            //request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.Proxy = new WebProxy("http://10.215.32.62:8080", true)
            {
                Credentials = new NetworkCredential("flsouza", "c6u4u6"),
                UseDefaultCredentials = false
            };
            //request.UseDefaultCredentials = true;

            if (method == "POST")
            {
                // Convert POST data to a byte array.
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
            }
            return request;
        }

        HttpWebResponse GetResponse(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                cookies.Add(response.Cookies);
                // Print the properties of each cookie.
                Console.WriteLine("\nCookies: ");
                foreach (Cookie cook in cookies.GetCookies(request.RequestUri))
                {
                    Console.WriteLine("Domain: {0}, String: {1}", cook.Domain, cook.ToString());
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("Web exception occurred. Status code: {0}", ex.Status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}
