using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Shared.Models
{
    public class SunfrogItem
    {
        [JsonProperty(PropertyName = "ArtOwnerID")]
        public int ArtOwnerID { get; set; }

        [JsonProperty(PropertyName = "IAgree")]
        public bool IAgree { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Keywords")]
        public string[] Keywords { get; set; }

        [JsonProperty(PropertyName = "imageFront")]
        public string ImageFront { get; set; }

        [JsonProperty(PropertyName = "imageBack")]
        public string ImageBack { get; set; }

        [JsonProperty(PropertyName = "types")]
        public SunfrogItemColor[] Types { get; set; }

        [JsonProperty(PropertyName = "images")]
        public SunfrogItemImage[] Images { get; set; }
    }

    public class SunfrogItemColor
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public string[] Colors { get; set; }
    }

    public class SunfrogItemImage
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }

    public class ProductsInfo
    {
        [JsonProperty(PropertyName = "Colors")]
        public JToken Colors { get; set; }

        [JsonProperty(PropertyName = "Products")]
        public JToken Products { get; set; }

        [JsonProperty(PropertyName = "ProductNames")]
        public JToken ProductNames { get; set; }

        [JsonProperty(PropertyName = "BasePrices")]
        public JToken BasePrices { get; set; }

        [JsonProperty(PropertyName = "ProductTypes")]
        public JToken ProductTypes { get; set; }

        [JsonProperty(PropertyName = "Design")]
        public JToken Design { get; set; }

        [JsonProperty(PropertyName = "Price")]
        public JToken Price { get; set; }

        public string GetProductName(string id)
        {
            if (GetProductType(id).Equals("case", StringComparison.OrdinalIgnoreCase))
            {
                return "Phone Case";
            }

            if (ProductNames != null)
            {
                foreach (var name in ProductNames)
                {
                    if (((JProperty) name).Name == id)
                    {
                        return ((JProperty) name).Value.ToObject<string>();
                    }
                }
            }

            return "";
        }

        public string GetProductType(string id)
        {
            if (ProductTypes != null)
            {
                foreach (var type in ProductTypes)
                {
                    foreach (var product in ((JProperty) type).Value.ToObject<JArray>())
                    {
                        if (product.ToObject<string>() == id)
                        {
                            return ((JProperty) type).Name;
                        }
                    }
                }
            }

            return "";
        }

        public JToken[] GetColors(string id)
        {
            if (Colors != null)
            {
                foreach (var color in Colors)
                {
                    if (((JProperty) color).Name == id)
                    {
                        var colors = new List<JToken>();
                        foreach (var colorCodes in ((JProperty) color).Value.ToObject<JToken>())
                        {
                            colors.Add(colorCodes);
                        }

                        return colors.ToArray();
                    }
                }
            }

            return new JToken[] { };
        }

        public decimal GetPrice(string id, bool bothSide)
        {
            if (Price != null)
            {
                var prices = Price[bothSide ? "2" : "1"];
                foreach (var price in prices)
                {
                    if (((JProperty) price).Name == id)
                    {
                        return ((JProperty) price).Value.ToObject<decimal>() / 100;
                    }
                }
            }

            return 0;
        }
    }

    public class TeechipItem
    {
        [JsonProperty(PropertyName = "launching")]
        public bool Launching { get; set; }

        [JsonProperty(PropertyName = "completedSteps")]
        public Dictionary<string, object> CompletedSteps => new Dictionary<string, object>
                                                            {
                                                                {"pick_products", true},
                                                                {"set_goal", true},
                                                                {"add_description", true}
                                                            };

        [JsonProperty(PropertyName = "design")]
        public string Design { get; set; }

        [JsonProperty(PropertyName = "designs")]
        public TeechipDesign[] Designs { get; set; }

        [JsonProperty(PropertyName = "estimatedProfit")]
        public double EstimatedProfit { get; set; }

        [JsonProperty(PropertyName = "products")]
        public TeechipProductItem[] Products { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "private")]
        public bool IsPrivate { get; set; }

        [JsonProperty(PropertyName = "isValid")]
        public Dictionary<string, object> IsValid => new Dictionary<string, object>
                                                     {
                                                         {"pick_products", true},
                                                         {"set_goal", true},
                                                         {"add_description", true}
                                                     };

        [JsonProperty(PropertyName = "step")]
        public string Step => "add_description";

        [JsonProperty(PropertyName = "goal")]
        public int Goal { get; set; }

        [JsonProperty(PropertyName = "waiting")]
        public bool Waiting { get; set; }

        [JsonProperty(PropertyName = "profit")]
        public JToken Profit { get; set; }

        [JsonProperty(PropertyName = "certified")]
        public bool Certified => true;

        [JsonProperty(PropertyName = "launchingProcessing")]
        public bool LaunchingProcessing => true;

        [JsonProperty(PropertyName = "autoRestart")]
        public bool AutoRestart { get; set; }
    }

    public class TeechipDesign
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "colorCount")]
        public int ColorCount { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public string[] Colors { get; set; }

        [JsonProperty(PropertyName = "side")]
        public string Side { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "cssSizing")]
        public TeechipCssSizing CssSizing { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public TeechipMetrics Metrics { get; set; }

        [JsonProperty(PropertyName = "fullColor")]
        public bool FullColor => true;

        [JsonProperty(PropertyName = "colorHash")]
        public JObject ColorHash => new JObject();

        [JsonProperty(PropertyName = "originalArtworkUrl")]
        public string OriginalArtworkUrl { get; set; }

        [JsonProperty(PropertyName = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty(PropertyName = "calculatedDimensions")]
        public TeechipDimensions CalculatedDimensions { get; set; }
    }

    public class TeechipCssSizing
    {
        [JsonProperty(PropertyName = "height")]
        public double Height { get; set; }

        [JsonProperty(PropertyName = "x")]
        public string X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public string Y { get; set; }
    }

    public class TeechipMetrics
    {
        [JsonProperty(PropertyName = "offsetTop")]
        public double OffsetTop { get; set; }

        [JsonProperty(PropertyName = "offsetLeft")]
        public double OffsetLeft { get; set; }

        [JsonProperty(PropertyName = "designHeight")]
        public double DesignHeight { get; set; }

        [JsonProperty(PropertyName = "designWidth")]
        public double DesignWidth { get; set; }
    }

    public class TeechipDimensions
    {
        [JsonProperty(PropertyName = "height")]
        public double Height { get; set; }

        [JsonProperty(PropertyName = "width")]
        public double Width { get; set; }
    }

    public class TeechipProductItem
    {
        [JsonProperty(PropertyName = "colors")]
        public string[] Colors { get; set; }

        [JsonProperty(PropertyName = "scalableId")]
        public string ScalableId { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "goal")]
        public int Goal { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "designs")]
        public TeechipDesign[] Designs { get; set; }

        [JsonProperty(PropertyName = "ppp")]
        public double PPP { get; set; }

        [JsonProperty(PropertyName = "pps")]
        public double PPS { get; set; }

        [JsonProperty(PropertyName = "orientation")]
        public string Orientation { get; set; }

        [JsonProperty(PropertyName = "hasBorder")]
        public bool HasBorder { get; set; }
    }

    public class TeespringItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "lookup_id")]
        public string LookupId { get; set; }

        [JsonProperty(PropertyName = "last_saved")]
        public string LastSaved { get; set; }

        [JsonProperty(PropertyName = "assets_generated")]
        public bool AssetsGenerated => false;

        [JsonProperty(PropertyName = "partnership")]
        public string Partnership { get; set; }

        [JsonProperty(PropertyName = "partnership_banner")]
        public string PartnershipBanner { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; } = "USD";

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; } = "USA";

        [JsonProperty(PropertyName = "additionalMerchSide")]
        public string AdditionalMerchSide { get; set; } = "front";

        [JsonProperty(PropertyName = "priorPrintableArea")]
        public string PriorPrintableArea { get; set; }

        [JsonProperty(PropertyName = "design")]
        public string Design { get; set; }

        [JsonProperty(PropertyName = "micro_id")]
        public string MicroId { get; set; }

        [JsonProperty(PropertyName = "launch_stage")]
        public string LaunchStage => "Details";

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "length")]
        public string Length { get; set; }

        [JsonProperty(PropertyName = "pickupinstructions")]
        public string PickupInstructions { get; set; } = "";

        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; } = "";

        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; } = "";

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; } = "";

        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; } = "";

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; } = "";

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = "";

        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; } = "";

        [JsonProperty(PropertyName = "category_id")]
        public string CategoryId { get; set; }

        [JsonProperty(PropertyName = "backDefaultSide")]
        public bool BackDefaultSide { get; set; } = false;

        [JsonProperty(PropertyName = "products")]
        public string Products { get; set; }

        [JsonProperty(PropertyName = "default_arts")]
        public string DefaultArts { get; set; } = "[]";

        [JsonProperty(PropertyName = "additional_merchandise")]
        public string AdditionalMerchandise { get; set; } = "[]";

        [JsonProperty(PropertyName = "client")]
        public string Client { get; set; } = "";

        [JsonProperty(PropertyName = "selling_price")]
        public double? SellingPrice { get; set; }

        [JsonProperty(PropertyName = "item_profit")]
        public double? ItemProfit { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public double? Cost { get; set; }

        [JsonProperty(PropertyName = "defaultProduct")]
        public string DefaultProduct { get; set; }

        [JsonProperty(PropertyName = "hadInvalidProductOption")]
        public bool HadInvalidProductOption => false;

        [JsonProperty(PropertyName = "pickup")]
        public bool Pickup => false;

        [JsonProperty(PropertyName = "tipping_point")]
        public int TippingPoint { get; set; }

        [JsonProperty(PropertyName = "facebook_snippet")]
        public string FacebookSnippet { get; set; }

        [JsonProperty(PropertyName = "store_ids")]
        public string StoreIds { get; set; } = "[]";

        [JsonProperty(PropertyName = "audience_ids")]
        public string AudienceIds { get; set; } = "[]";

        [JsonProperty(PropertyName = "recentArts")]
        public string RecentArts { get; set; } = "[]";

        [JsonProperty(PropertyName = "stage")]
        public string Stage { get; set; } = "details";

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        [JsonProperty(PropertyName = "auto_relaunch")]
        public string AutoRelaunch { get; set; } = null;

        [JsonProperty(PropertyName = "flaggedKeywords")]
        public string FlaggedKeywords { get; set; } = "[]";

        [JsonProperty(PropertyName = "donation_enabled")]
        public bool DonationEnabled { get; set; } = false;

        [JsonProperty(PropertyName = "charity_id")]
        public string CharityId { get; set; } = "";

        [JsonProperty(PropertyName = "donation_percent")]
        public int DonationPercent { get; set; } = 100;

        [JsonProperty(PropertyName = "emoji_one_use_count")]
        public int EmojiOneUseCount { get; set; } = 0;

        [JsonProperty(PropertyName = "system_tags")]
        public string SystemTags { get; set; } = "[]";
    }

    public class TeespringProductItem
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public int[] Colors { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; } = "USD";

        [JsonProperty(PropertyName = "orientation")]
        public string Orientation { get; set; } = "Portrait";
    }

    public class TeespringSides
    {
        [JsonProperty(PropertyName = "front")]
        public TeespringDesignSide Front { get; set; } = new TeespringDesignSide();

        [JsonProperty(PropertyName = "back")]
        public TeespringDesignSide Back { get; set; } = new TeespringDesignSide();
    }

    public class TeespringDesign
    {
        [JsonProperty(PropertyName = "lookupId")]
        public string LookupId { get; set; }

        [JsonProperty(PropertyName = "sides")]
        public TeespringSides Sides { get; set; } = new TeespringSides();

        [JsonProperty(PropertyName = "activeSide")]
        public string ActiveSide { get; set; } = "front";

        [JsonProperty(PropertyName = "high_quality_artwork")]
        public bool HighQualityArtwork { get; set; } = true;

        [JsonProperty(PropertyName = "crap_quality_artwork")]
        public bool CrapQualityArtwork { get; set; } = false;

        [JsonProperty(PropertyName = "product_type_id")]
        public int ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "frontColors")]
        public int FrontColors { get; set; }

        [JsonProperty(PropertyName = "flashFront")]
        public bool FlashFront { get; set; } = false;

        [JsonProperty(PropertyName = "backColors")]
        public int BackColors { get; set; }

        [JsonProperty(PropertyName = "flashBack")]
        public bool FlashBack { get; set; } = false;
    }

    public class TeespringColor
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "origin")]
        public object Origin { get; set; } = null;

        [JsonProperty(PropertyName = "heather")]
        public bool Heather { get; set; } = false;

        [JsonProperty(PropertyName = "texture")]
        public string Texture { get; set; } = "";
    }

    public class TeespringDesignSide
    {
        [JsonProperty(PropertyName = "removeLayerBinding")]
        public JObject RemoveLayerBinding { get; set; } = new JObject();

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "lookupId")]
        public string LookupId { get; set; }

        [JsonProperty(PropertyName = "layers")]
        public TeespringDesignLayer[] Layers { get; set; } = { };

        [JsonProperty(PropertyName = "bbox")]
        public TeespringBox BBox { get; set; } = new TeespringBox();

        [JsonProperty(PropertyName = "absoluteBBox")]
        public TeespringBox AbsoluteBBox { get; set; } = new TeespringBox();

        //[JsonProperty(PropertyName = "beingViewed")]
        //public bool BeingViewed { get; set; } = false;

        [JsonProperty(PropertyName = "initialFreeTransformAttrs")]
        public object InitialFreeTransformAttrs { get; set; } = null;

        [JsonProperty(PropertyName = "initialPrintableArea")]
        public object InitialPrintableArea { get; set; } = null;

        [JsonProperty(PropertyName = "referencePoint")]
        public Dictionary<string, object> ReferencePoint { get; set; } = new Dictionary<string, object>
                                                                         {
                                                                             {"x", 0},
                                                                             {"y", 0}
                                                                         };

        [JsonProperty(PropertyName = "ppi")]
        public double PPI { get; set; }

        [JsonProperty(PropertyName = "editable")]
        public object Editable { get; set; } = null;

        [JsonProperty(PropertyName = "sequence")]
        public int Sequence { get; set; } = 1;

        [JsonProperty(PropertyName = "priorPrintableBox")]
        public string PriorPrintableBox { get; set; }

        [JsonProperty(PropertyName = "scaleAndMove")]
        public object ScaleAndMove { get; set; } = null;

        [JsonProperty(PropertyName = "colors")]
        public TeespringColor[] Colors { get; set; }

        [JsonProperty(PropertyName = "rasterImageInfo")]
        public Dictionary<string, object> RasterImageInfo { get; set; } = new Dictionary<string, object>
                                                                          {
                                                                              {"width", 0},
                                                                              {"height", 0},
                                                                              {"format", "png"}
                                                                          };

        [JsonProperty(PropertyName = "svg")]
        public string Svg { get; set; }
    }

    public class TeespringBox
    {
        [JsonProperty(PropertyName = "x")]
        public double? X { get; set; } = null;

        [JsonProperty(PropertyName = "y")]
        public double? Y { get; set; } = null;

        [JsonProperty(PropertyName = "width")]
        public double? Width { get; set; } = null;

        [JsonProperty(PropertyName = "height")]
        public double? Height { get; set; } = null;
    }

    public class TeespringDesignLayer
    {
        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "originalFilename")]
        public string OriginalFileName { get; set; }

        [JsonProperty(PropertyName = "sourceFilename")]
        public string SourceFileName { get; set; }

        [JsonProperty(PropertyName = "backgroundRemovedFilename")]
        public string BackgroundRemovedFilename { get; set; }

        [JsonProperty(PropertyName = "tempFilename")]
        public string TempFilename { get; set; }

        [JsonProperty(PropertyName = "ratio")]
        public object Ratio { get; set; } = null;

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "initialScale")]
        public double InitialScale { get; set; } = 1;

        [JsonProperty(PropertyName = "transformations")]
        public JArray Transformations { get; set; } = new JArray();

        [JsonProperty(PropertyName = "matrix")]
        public TeespringMatrix Matrix { get; set; } = new TeespringMatrix();

        [JsonProperty(PropertyName = "colors")]
        public TeespringColor[] Colors { get; set; }

        [JsonProperty(PropertyName = "minimumPPI")]
        public object MinimumPPI { get; set; } = null;

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "beingEdited")]
        public bool BeingEdited { get; set; } = false;

        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; } = "Upload";

        [JsonProperty(PropertyName = "hasPavlovFlags")]
        public bool HasPavlovFlags { get; set; } = false;

        [JsonProperty(PropertyName = "pavlovId")]
        public string PavlovId { get; set; } = "";

        [JsonProperty(PropertyName = "absoluteBBox")]
        public TeespringBox AbsoluteBBox { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = "Image";

        [JsonProperty(PropertyName = "bbox")]
        public TeespringBox BBox { get; set; }

        [JsonProperty(PropertyName = "preserve-colors")]
        public bool PreserveColors { get; set; } = true;

        [JsonProperty(PropertyName = "fillColor")]
        public object FillColor { get; set; } = null;

        [JsonProperty(PropertyName = "outlineThickness")]
        public int OutlineThickness { get; set; } = 0;

        [JsonProperty(PropertyName = "outlineColor")]
        public TeespringColor OutlineColor { get; set; } = null;

        [JsonProperty(PropertyName = "newDesignData")]
        public Dictionary<string, object> NewDesignData { get; set; }
    }

    public class TeespringMatrix
    {
        [JsonProperty(PropertyName = "a")]
        public double A { get; set; }

        [JsonProperty(PropertyName = "b")]
        public double B { get; set; }

        [JsonProperty(PropertyName = "c")]
        public double C { get; set; }

        [JsonProperty(PropertyName = "d")]
        public double D { get; set; }

        [JsonProperty(PropertyName = "e")]
        public double E { get; set; }

        [JsonProperty(PropertyName = "f")]
        public double F { get; set; }
    }

    public class TeespringNewItem
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "products")]
        public List<TeespringNewItemProduct> Products { get; set; } = new List<TeespringNewItemProduct>();

        [JsonProperty(PropertyName = "featured_product")]
        public Dictionary<string, string> FeaturedProduct { get; set; } = new Dictionary<string, string>
                                                                          {
                                                                              {"color", ""},
                                                                              {"id", ""}
                                                                          };
    }

    public class TeespringNewItemProduct
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "featured_side")]
        public string FeaturedSide { get; set; }

        [JsonProperty(PropertyName = "mockups")]
        public List<TeespringNewProductMockup> Mockups { get; set; } = new List<TeespringNewProductMockup>();

        [JsonProperty(PropertyName = "customizations")]
        public List<TeespringNewProductCustomization> Customizations { get; set; } = new List<TeespringNewProductCustomization>();

        [JsonProperty(PropertyName = "prices")]
        public List<TeespringNewProductPrice> Prices { get; set; } = new List<TeespringNewProductPrice>();

        [JsonProperty(PropertyName = "retail_price")]
        public Dictionary<string, object> RetailPrice { get; set; } = new Dictionary<string, object>()
                                                                      {
                                                                          {"cents", 0},
                                                                          {"currency", "USD"}
                                                                      };
    }

    public class TeespringNewProductCustomization
    {
        [JsonProperty(PropertyName = "product_template_id")]
        public string ProductTemplateId { get; set; }

        [JsonProperty(PropertyName = "svg")]
        public string Svg { get; set; }
    }

    public class TeespringNewProductPrice
    {
        [JsonProperty(PropertyName = "product_size_id")]
        public string ProductSizeId { get; set; }

        [JsonProperty(PropertyName = "retail_price")]
        public Dictionary<string, object> RetailPrice { get; set; } = new Dictionary<string, object>()
                                                                      {
                                                                          {"cents", 0},
                                                                          {"currency", "USD"}
                                                                      };
    }

    public class TeespringNewProductMockup
    {
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "views")]
        public List<TeespringNewProductMockupView> Views { get; set; } = new List<TeespringNewProductMockupView>();
    }

    public class TeespringNewProductMockupView
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class ViralstyleItem
    {
        [JsonProperty(PropertyName = "isDraft")]
        public bool IsDraft { get; set; } = false;

        [JsonProperty(PropertyName = "campaignId")]
        public string CampaignId { get; set; }

        [JsonProperty(PropertyName = "createSimilar")]
        public bool CreateSimilar { get; set; } = false;

        [JsonProperty(PropertyName = "campaignUniqueId")]
        public string CampaignUniqueId { get; set; }

        [JsonProperty(PropertyName = "imageIdentifier")]
        public string ImageIdentifier { get; set; }

        [JsonProperty(PropertyName = "goal")]
        public int Goal { get; set; }

        [JsonProperty(PropertyName = "mainProduct")]
        public ViralstyleProduct MainProduct { get; set; }

        [JsonProperty(PropertyName = "pricing")]
        public Dictionary<string, object> Pricing { get; set; } = new Dictionary<string, object>
                                                                  {
                                                                      {"basePrice", 0},
                                                                      {"sellingPrice", 0},
                                                                      {"dtgPrice", 0}
                                                                  };

        [JsonProperty(PropertyName = "additionalProducts")]
        public ViralstyleAdditionalProduct[] AdditionalProducts { get; set; }

        [JsonProperty(PropertyName = "campaign_name")]
        public string CampaignName { get; set; }

        [JsonProperty(PropertyName = "campaign_description")]
        public string CampaignDescription { get; set; }

        [JsonProperty(PropertyName = "campaign_url")]
        public string CampaignUrl { get; set; }

        [JsonProperty(PropertyName = "campaign_tags")]
        public string[] CampaignTags { get; set; }

        [JsonProperty(PropertyName = "campaign_length")]
        public int CampaignLength { get; set; }

        [JsonProperty(PropertyName = "campaign_end_date")]
        public string CampaignEndDate { get; set; }

        [JsonProperty(PropertyName = "campaign_end_date_obj")]
        public string CampaignEndDateObj { get; set; }

        [JsonProperty(PropertyName = "campaign_end_date_utc")]
        public long CampaignEndDateUtc { get; set; }

        [JsonProperty(PropertyName = "hide_marketplace")]
        public bool HideMarketplace { get; set; } = false;

        [JsonProperty(PropertyName = "campaign_auto_relaunch")]
        public bool CampaignAutoRelaunch { get; set; }

        [JsonProperty(PropertyName = "campaign_page_timer")]
        public bool CampaignPageTimer { get; set; } = true;

        [JsonProperty(PropertyName = "campaign_show_goal")]
        public bool CampaignShowGoal { get; set; }

        [JsonProperty(PropertyName = "campaign_auto_extend")]
        public bool CampaignAutoExtend { get; set; } = true;

        [JsonProperty(PropertyName = "campaign_dtg_auto_goal_drop")]
        public bool CampaignDtgAutoGoalDrop { get; set; } = true;

        [JsonProperty(PropertyName = "campaign_show_back_default")]
        public bool CampaignShowBackDefault { get; set; } = false;

        [JsonProperty(PropertyName = "campaign_upsells")]
        public string[] CampaignUpsells { get; set; } = new string[] { };

        [JsonProperty(PropertyName = "colors")]
        public Dictionary<string, object> Colors { get; set; } = new Dictionary<string, object>
                                                                 {
                                                                     {"totalColors", new List<Dictionary<string, object>>()},
                                                                     {"totalFrontColors", new List<Dictionary<string, object>>()},
                                                                     {"totalBackColors", new List<Dictionary<string, object>>()}
                                                                 };

        [JsonProperty(PropertyName = "design")]
        public Dictionary<string, object> Design { get; set; } = new Dictionary<string, object>
                                                                 {
                                                                     {
                                                                         "front", new Dictionary<string, object>
                                                                                  {
                                                                                      {"original", null},
                                                                                      {"upscaled", null},
                                                                                      {
                                                                                          "offsets", new Dictionary<string, double>
                                                                                                     {
                                                                                                         {"top", 0},
                                                                                                         {"left", 0}
                                                                                                     }
                                                                                      }
                                                                                  }
                                                                     },
                                                                     {
                                                                         "back", new Dictionary<string, object>
                                                                                 {
                                                                                     {"original", null},
                                                                                     {"upscaled", null},
                                                                                     {
                                                                                         "offsets", new Dictionary<string, double>
                                                                                                    {
                                                                                                        {"top", 0},
                                                                                                        {"left", 0}
                                                                                                    }
                                                                                     }
                                                                                 }
                                                                     }
                                                                 };

        [JsonProperty(PropertyName = "fonts")]
        public Dictionary<string, object> Fonts { get; set; } = new Dictionary<string, object>
                                                                {
                                                                    {
                                                                        "front", new List<Dictionary<string, object>>()
                                                                                 {
                                                                                     new Dictionary<string, object>
                                                                                     {
                                                                                         {"font", "Exo"},
                                                                                         {"times", 1}
                                                                                     }
                                                                                 }
                                                                    },
                                                                    {
                                                                        "back", new List<Dictionary<string, object>>()
                                                                                {
                                                                                    new Dictionary<string, object>
                                                                                    {
                                                                                        {"font", "Exo"},
                                                                                        {"times", 1}
                                                                                    }
                                                                                }
                                                                    }
                                                                };

        [JsonProperty(PropertyName = "purchases")]
        public Dictionary<string, object> Purchases { get; set; } = new Dictionary<string, object>
                                                                    {
                                                                        {"products", new object[] { }},
                                                                        {
                                                                            "buyer", new Dictionary<string, object>
                                                                                     {
                                                                                         {"email", ""},
                                                                                         {"name", ""},
                                                                                         {"address", ""},
                                                                                         {"apt", ""},
                                                                                         {"city", ""},
                                                                                         {"state", ""},
                                                                                         {"zip", ""},
                                                                                         {"country", ""}
                                                                                     }
                                                                        },
                                                                        {
                                                                            "payment", new Dictionary<string, object>
                                                                                       {
                                                                                           {
                                                                                               "cc", new Dictionary<string, object>
                                                                                                     {
                                                                                                         {"token", ""},
                                                                                                         {"cc_bin", ""},
                                                                                                         {"currency", "USD"}
                                                                                                     }
                                                                                           }
                                                                                       }
                                                                        }
                                                                    };

        [JsonProperty(PropertyName = "accept_terms")]
        public bool AcceptTerms { get; set; } = true;

        [JsonProperty(PropertyName = "api_order")]
        public bool ApiOrder { get; set; } = false;

        [JsonProperty(PropertyName = "manual_order")]
        public bool ManualOrder { get; set; } = false;

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; } = "";

        [JsonProperty(PropertyName = "force_dtg")]
        public bool ForceDtg { get; set; } = false;

        [JsonProperty(PropertyName = "auto_import_to_shopify")]
        public bool AutoImportToShopify { get; set; } = false;

        [JsonProperty(PropertyName = "auto_set_visibility")]
        public bool AutoSetVisibility { get; set; } = false;

        [JsonProperty(PropertyName = "import_back_images")]
        public bool ImportBackImages { get; set; } = true;

        [JsonProperty(PropertyName = "shopify_collection")]
        public string ShopifyCollection { get; set; } = "";

        [JsonProperty(PropertyName = "shopifyDomain")]
        public string ShopifyDomain { get; set; } = "";
    }

    public class ViralstyleProduct
    {
        [JsonProperty(PropertyName = "currentColor")]
        public ViralstyleColor CurrentColor { get; set; }

        [JsonProperty(PropertyName = "product")]
        public ViralstyleProductItem Product { get; set; }

        [JsonProperty(PropertyName = "front")]
        public Dictionary<string, List<ViralstyleDesign>> Front { get; set; } = new Dictionary<string, List<ViralstyleDesign>>()
                                                                                {
                                                                                    {"designerItems", new List<ViralstyleDesign>()}
                                                                                };

        [JsonProperty(PropertyName = "back")]
        public Dictionary<string, List<ViralstyleDesign>> Back { get; set; } = new Dictionary<string, List<ViralstyleDesign>>()
                                                                               {
                                                                                   {"designerItems", new List<ViralstyleDesign>()}
                                                                               };

        [JsonProperty(PropertyName = "product_id")]
        public int ProductId { get; set; }
    }

    public class ViralstyleColor
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "product_id")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "ab_color")]
        public string AbColor { get; set; }

        [JsonProperty(PropertyName = "sm_color")]
        public string SmColor { get; set; }

        [JsonProperty(PropertyName = "dtg2go_color")]
        public string Dtg2GoColor { get; set; }

        [JsonProperty(PropertyName = "dtg2go_style_code")]
        public string Dtg2GoStyleCode { get; set; }

        [JsonProperty(PropertyName = "ab_style_code")]
        public string AbStyleCode { get; set; }

        [JsonProperty(PropertyName = "hex")]
        public string Hex { get; set; }

        [JsonProperty(PropertyName = "active")]
        public int Active { get; set; }

        [JsonProperty(PropertyName = "common_color_id")]
        public string CommonColorId { get; set; }

        [JsonProperty(PropertyName = "heather_image_url")]
        public string HeatherImageUrl { get; set; }

        [JsonProperty(PropertyName = "heather_image_icon_url")]
        public string HeatherImageIconUrl { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "hsl")]
        public Dictionary<string, object> Hsl { get; set; }
    }

    public class ViralstyleProductItem
    {
        [JsonProperty(PropertyName = "image")]
        public Dictionary<string, object> Image { get; set; } = new Dictionary<string, object>
                                                                {
                                                                    {"front", new ViralstyleImage()},
                                                                    {"back", new ViralstyleImage()}
                                                                };

        [JsonProperty(PropertyName = "product_thumbnail_image")]
        public string ProductThumbnailImage { get; set; }

        [JsonProperty(PropertyName = "product_image")]
        public string ProductImage { get; set; }

        [JsonProperty(PropertyName = "mock_index")]
        public int MockIndex { get; set; } = -1;

        [JsonProperty(PropertyName = "mock_id")]
        public int MockId { get; set; } = -1;

        [JsonProperty(PropertyName = "product_type")]
        public string ProductType { get; set; } = "PRODUCT";

        [JsonProperty(PropertyName = "dimensions")]
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Dimensions { get; set; } =
            new Dictionary<string, Dictionary<string, Dictionary<string, string>>>()
            {
                {
                    "front", new Dictionary<string, Dictionary<string, string>>()
                             {
                                 {
                                     "offsets", new Dictionary<string, string>()
                                                {
                                                    {"height", "0px"},
                                                    {"width", "0px"},
                                                    {"left", "0px"},
                                                    {"top", "0px"}
                                                }
                                 },
                                 {
                                     "image", new Dictionary<string, string>()
                                              {
                                                  {"width", "0px"},
                                                  {"height", "0px"}
                                              }
                                 }
                             }
                },
                {
                    "back", new Dictionary<string, Dictionary<string, string>>()
                            {
                                {
                                    "offsets", new Dictionary<string, string>()
                                               {
                                                   {"height", "0px"},
                                                   {"width", "0px"},
                                                   {"left", "0px"},
                                                   {"top", "0px"}
                                               }
                                },
                                {
                                    "image", new Dictionary<string, string>()
                                             {
                                                 {"width", "0px"},
                                                 {"height", "0px"}
                                             }
                                }
                            }
                }
            };

        [JsonProperty(PropertyName = "promo_product_skus")]
        public string[] PromoProductSkus { get; set; } = { "G5000" };

        [JsonProperty(PropertyName = "promo_text_product")]
        public string PromoTextProduct { get; set; } = "PROMO";

        //[JsonProperty(PropertyName = "product")]
        //public JToken Product { get; set; }
    }

    public class ViralstyleImage
    {
        [JsonProperty(PropertyName = "original")]
        public string Original { get; set; }

        [JsonProperty(PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty(PropertyName = "large")]
        public string Large { get; set; }
    }

    public class ViralstyleDesign
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = "IMAGE";

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "imageData")]
        public Dictionary<string, object> ImageData { get; set; } = new Dictionary<string, object>
                                                                    {
                                                                        {"original_url", ""},
                                                                        {"thumbnail_url", ""},
                                                                        {"original_upload_url", ""},
                                                                        {"height", 0},
                                                                        {"width", 0},
                                                                        {"left", 0},
                                                                        {"top", 0}
                                                                    };

        [JsonProperty(PropertyName = "colors")]
        public List<string> Colors { get; set; }

        [JsonProperty(PropertyName = "allow_delete")]
        public bool AllowDelete { get; set; } = true;

        [JsonProperty(PropertyName = "span")]
        public Dictionary<string, object> Span { get; set; } = new Dictionary<string, object>()
                                                               {
                                                                   {"height", 0},
                                                                   {"width", 0},
                                                                   {"top", 0},
                                                                   {"left", 0},
                                                                   {"rotLeft", 0},
                                                                   {"rotTop", 0},
                                                                   {"index", 0}
                                                               };

        [JsonProperty(PropertyName = "element")]
        public Dictionary<string, object> Element { get; set; } = new Dictionary<string, object>
                                                                  {
                                                                      {"x", 0},
                                                                      {"y", 0}
                                                                  };

        [JsonProperty(PropertyName = "rotate")]
        public double Rotate { get; set; } = 0;

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; } = 0;
    }

    public class ViralstyleAdditionalProduct
    {
        [JsonProperty(PropertyName = "product")]
        public int Product { get; set; }

        [JsonProperty(PropertyName = "color")]
        public int Color { get; set; }

        [JsonProperty(PropertyName = "hex")]
        public string Hex { get; set; }

        [JsonProperty(PropertyName = "selling_price")]
        public string SellingPrice { get; set; }

        [JsonProperty(PropertyName = "profit")]
        public string Profit { get; set; } = "NaN";

        [JsonProperty(PropertyName = "base_price")]
        public decimal BasePrice { get; set; }

        [JsonProperty(PropertyName = "dtg_price")]
        public decimal DtgPrice { get; set; }

        [JsonProperty(PropertyName = "dtg_profit")]
        public string DtgProfit { get; set; } = "NaN";
    }

    public class TeezilyItem
    {
        [JsonProperty(PropertyName = "address_description")]
        public string AddressDescription { get; set; } = null;

        [JsonProperty(PropertyName = "closed")]
        public bool Closed { get; set; } = false;

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "created_with")]
        public string CreatedWith { get; set; } = "tee_app_fabric_1.5.0";

        [JsonProperty(PropertyName = "currency_exchange_rate")]
        public Dictionary<string, decimal> CurrencyExchangeRate { get; set; } = null;

        [JsonProperty(PropertyName = "customized")]
        public bool Customized { get; set; } = false;

        [JsonProperty(PropertyName = "days_limit")]
        public int? DaysLimit { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "drafted_at")]
        public string DraftedAt { get; set; } = null;

        [JsonProperty(PropertyName = "drawable_areas")]
        public List<TeezilyDrawableArea> DrawableAreas { get; set; }

        [JsonProperty(PropertyName = "enable_shipping_on_address")]
        public bool EnableShippingOnAddress { get; set; } = false;

        [JsonProperty(PropertyName = "first_item")]
        public bool? FirstItem { get; set; } = null;

        [JsonProperty(PropertyName = "funded_count")]
        public int FundedCount { get; set; } = 0;

        [JsonProperty(PropertyName = "has_address")]
        public bool HasAddress { get; set; } = false;

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "in_marketplace")]
        public bool InMarketplace { get; set; }

        [JsonProperty(PropertyName = "is_orderable")]
        public bool IsOrderable { get; set; } = false;

        [JsonProperty(PropertyName = "items")]
        public List<TeezilyGarmentItem> Items { get; set; }

        [JsonProperty(PropertyName = "last_next_campaign_reference")]
        public string LastNextCampaignReference { get; set; }

        [JsonProperty(PropertyName = "links")]
        public Dictionary<string, string> Links { get; set; }

        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; } = null;

        [JsonProperty(PropertyName = "marketplace_category_ids")]
        public List<int> MarketplaceCategoryIds { get; set; } = null;

        [JsonProperty(PropertyName = "max_available_items")]
        public int MaxAvailableItems { get; set; } = 100;

        [JsonProperty(PropertyName = "minimum_sales_goal")]
        public int MinimumSalesGoal { get; set; } = 1;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "publish_token")]
        public string PublishedToken { get; set; }

        [JsonProperty(PropertyName = "published_at")]
        public string PublishedAt { get; set; } = null;

        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }

        [JsonProperty(PropertyName = "relaunchable")]
        public bool Relaunchable { get; set; }

        [JsonProperty(PropertyName = "relaunching")]
        public bool Relaunching { get; set; } = false;

        [JsonProperty(PropertyName = "sales_goal")]
        public int? SalesGoal { get; set; }

        [JsonProperty(PropertyName = "show_store")]
        public bool ShowStore { get; set; } = false;

        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }

        [JsonProperty(PropertyName = "unavailable_items")]
        public List<int> UnavailableItems { get; set; }

        [JsonProperty(PropertyName = "until")]
        public string Until { get; set; } = null;

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "visible")]
        public bool Visible { get; set; } = false;

        [JsonProperty(PropertyName = "hashtags")]
        public string[] HashTags { get; set; }

        public TeezilyItem(JToken json)
        {
            try
            {
                AddressDescription = json["address_description"]?.ToObject<string>();
                Closed = json["closed"]?.ToObject<bool?>() ?? false;
                Currency = json["currency"]?.ToObject<string>();
                CurrencyExchangeRate = new Dictionary<string, decimal>();
                foreach (var rate in json["currency_exchange_rate"].ToObject<JToken>().Children())
                {
                    CurrencyExchangeRate.Add(((JProperty)rate).Name, rate.ToObject<decimal>());
                }
                Customized = json["customized"]?.ToObject<bool?>() ?? false;
                DaysLimit = json["days_limit"]?.ToObject<int?>();
                Description = json["description"]?.ToObject<string>();
                DraftedAt = json["drafted_at"]?.ToObject<string>();
                DrawableAreas = new List<TeezilyDrawableArea>();
                FirstItem = json["first_item"]?.ToObject<bool?>();
                FundedCount = json["funded_count"]?.ToObject<int?>() ?? 0;
                HasAddress = json["has_address"]?.ToObject<bool?>() ?? false;
                Id = json["id"]?.ToObject<long>() ?? 0;
                InMarketplace = json["in_marketplace"]?.ToObject<bool?>() ?? true;
                IsOrderable = json["is_orderable"]?.ToObject<bool?>() ?? false;
                Items = new List<TeezilyGarmentItem>();
                LastNextCampaignReference = json["last_next_campaign_reference"]?.ToObject<string>();
                Links = new Dictionary<string, string>
                        {
                            {"analytics", json["links"]["analytics"]?.ToObject<string>()},
                            {"create_similar", json["links"]["create_similar"]?.ToObject<string>()},
                            {"edit", json["links"]["edit"]?.ToObject<string>()},
                            {"show", json["links"]["show"]?.ToObject<string>()}
                        };
                Locale = json["locale"]?.ToObject<string>();
                MarketplaceCategoryIds = new List<int>();
                MaxAvailableItems = json["max_available_items"]?.ToObject<int?>() ?? 30;
                MinimumSalesGoal = json["minimum_sales_goal"]?.ToObject<int?>() ?? 1;
                Name = json["name"]?.ToObject<string>();
                PublishedAt = json["published_at"]?.ToObject<string>();
                Reference = json["reference"]?.ToObject<string>();
                Relaunchable = json["relaunchable"]?.ToObject<bool?>() ?? false;
                Relaunching = json["relaunching"]?.ToObject<bool?>() ?? false;
                SalesGoal = json["sales_goal"]?.ToObject<int?>();
                ShowStore = json["show_store"]?.ToObject<bool?>() ?? false;
                Slug = json["slug"]?.ToObject<string>();
                UnavailableItems = new List<int>();
                Until = json["until"]?.ToObject<string>();
                Url = json["url"]?.ToObject<string>();
                Visible = json["visible"]?.ToObject<bool?>() ?? false;
            }
            catch (Exception exc)
            {
            }
        }
    }

    public class TeezilyGarmentItem
    {
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "compare_at_price")]
        public decimal CompareAtPrice { get; set; } = 0;

        [JsonProperty(PropertyName = "product_id")]
        public int ProductId { get; set; }
    }

    public class TeezilyDrawableArea
    {
        [JsonProperty(PropertyName = "image_base64")]
        public string Base64Image { get; set; }

        [JsonProperty(PropertyName = "save_object")]
        public List<TeezilySaveObject> SaveObject { get; set; }

        [JsonProperty(PropertyName = "side_id")]
        public int SideId { get; set; }
    }

    public class TeezilySaveObject
    {
        [JsonProperty(PropertyName = "MINIMUM_HEIGHT_FOR_REMOVE")]
        public int MinimumHeightForRemove { get; set; } = 8;

        [JsonProperty(PropertyName = "MINIMUM_WIDTH_FOR_REMOVE")]
        public int MinimumWidthForRemove { get; set; } = 8;

        [JsonProperty(PropertyName = "fabricObject")]
        public TeezilyFabricObject FabricObject { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "grid")]
        public string Grid { get; set; } = null;

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "resolution")]
        public Dictionary<string, int> Resolution { get; set; }

        [JsonProperty(PropertyName = "sha1")]
        public string Sha1 { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = "TImage";

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "wasAnalyzed")]
        public bool WasAnalyzed { get; set; } = true;
    }

    public class TeezilyFabricObject
    {
        [JsonProperty(PropertyName = "alignX")]
        public string AlignX { get; set; } = "none";

        [JsonProperty(PropertyName = "alignY")]
        public string AlignY { get; set; } = "none";

        [JsonProperty(PropertyName = "angle")]
        public int Angle { get; set; } = 0;

        [JsonProperty(PropertyName = "backgroundColor")]
        public string BackgroundColor { get; set; } = "";

        [JsonProperty(PropertyName = "crossOrigin")]
        public string CrossOrigin { get; set; } = "Anonymous";

        [JsonProperty(PropertyName = "fill")]
        public string Fill { get; set; } = "rgb(0,0,0)";

        [JsonProperty(PropertyName = "fillRule")]
        public string FillRule { get; set; } = "nonzero";

        [JsonProperty(PropertyName = "filters")]
        public string[] Filters { get; set; } = { };

        [JsonProperty(PropertyName = "flipX")]
        public bool FlipX { get; set; } = false;

        [JsonProperty(PropertyName = "flipY")]
        public bool FlipY { get; set; } = false;

        [JsonProperty(PropertyName = "globalCompositeOperation")]
        public string GlobalCompositeOperation { get; set; } = "source-over";

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "left")]
        public int Left { get; set; }

        [JsonProperty(PropertyName = "meetOrSlice")]
        public string MeetOrSlice { get; set; } = "meet";

        [JsonProperty(PropertyName = "opacity")]
        public int Opacity { get; set; } = 1;

        [JsonProperty(PropertyName = "originX")]
        public string OriginX { get; set; } = "center";

        [JsonProperty(PropertyName = "originY")]
        public string OriginY { get; set; } = "center";

        [JsonProperty(PropertyName = "scaleX")]
        public double ScaleX { get; set; }

        [JsonProperty(PropertyName = "scaleY")]
        public double ScaleY { get; set; }

        [JsonProperty(PropertyName = "shadow")]
        public string Shadow { get; set; } = null;

        [JsonProperty(PropertyName = "stroke")]
        public string Stroke { get; set; } = null;

        [JsonProperty(PropertyName = "strokeDashArray")]
        public string StrokeDashArray { get; set; } = null;

        [JsonProperty(PropertyName = "strokeLineCap")]
        public string StrokeLineCap { get; set; } = "butt";

        [JsonProperty(PropertyName = "strokeLineJoin")]
        public string StrokeLineJoin { get; set; } = "miter";

        [JsonProperty(PropertyName = "strokeMiterLimit")]
        public int StrokeMiterLimit { get; set; } = 10;

        [JsonProperty(PropertyName = "strokeWidth")]
        public int StrokeWidth { get; set; } = 1;

        [JsonProperty(PropertyName = "top")]
        public int Top { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = "image";

        [JsonProperty(PropertyName = "visible")]
        public bool Visible { get; set; } = true;

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
    }
    
    public class Point3D
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }
    }

    public class Light
    {
        public int Color { get; set; }

        public int BackgroundColor { get; set; }

        public float Intensity { get; set; }

        public Point3D Position { get; set; }

        public string Type { get; set; }
    }
}
