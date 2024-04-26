use serde::Deserialize;

#[derive(Deserialize, Debug)]
struct JsonEndpoint {
    method: Option<String>,
    methods: Option<Vec<String>>,
    response: Option<String>,
    response_generator: Option<String>,
}

#[derive(Debug, Clone)]
pub struct MockEndpoint {
    pub route: String,
    pub response_generator: Option<String>,
    pub response: Option<String>,
    pub methods: Vec<String>,
}

pub mod json_parser {
    use super::{JsonEndpoint, MockEndpoint};
    use std::collections::HashMap;

    pub fn parse(json: &str) -> Vec<MockEndpoint> {
        let endpoints: HashMap<String, JsonEndpoint> =
            serde_json::from_str(json).unwrap_or_default();

        endpoints
            .into_iter()
            .map(|(route, endpoint)| MockEndpoint {
                route,
                response_generator: endpoint.response_generator,
                response: endpoint.response,
                methods: match endpoint.method {
                    Some(method) => vec![method],
                    None => endpoint.methods.unwrap_or_default(),
                },
            })
            .collect()
    }
}
