pub mod json_parser;
use json_parser::json_parser::parse;

use std::{
    fs::{self},
    path::Path,
};

use actix_web::{web, App, HttpResponse, HttpServer, Responder, Route};
use clap::Parser;

use crate::json_parser::MockEndpoint;

#[derive(Parser, Debug)]
#[command(version, about, long_about = None)]
struct Args {
    #[clap(flatten)]
    input: ArgGroup,
}

#[derive(Debug, clap::Args)]
#[group(required = true, multiple = false)]
pub struct ArgGroup {
    #[arg(short, long)]
    file: Option<String>,

    #[arg(short, long)]
    directory: Option<String>,
}

async fn generate_response(response: Option<String>) -> impl Responder {
    match response {
        Some(resp) => HttpResponse::Ok().body(resp),
        None => HttpResponse::InternalServerError().body("Internal Server Error"),
    }
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    let args = Args::parse();

    let mut files: Vec<String> = Vec::new();
    if args.input.file != None {
        files.push(args.input.file.unwrap())
    } else {
        let mut json_files = get_json_files(args.input.directory.unwrap());
        files.append(&mut json_files)
    }

    let mut all_endpoints: Vec<MockEndpoint> = Vec::new();

    for file in &files {
        let contents = fs::read_to_string(file).unwrap();
        let endpoints = parse(&contents);
        all_endpoints.append(&mut endpoints.clone());
    }

    println!("Registering {} endpoints.", all_endpoints.len());

    let app = move || {
        let mut app = App::new();

        let endpoints = &all_endpoints;
        for endpoint in endpoints {
            for method in &endpoint.methods {
                let response = endpoint.response.clone();
                app = app.route(
                    &endpoint.route,
                    get_web_method(&method)
                        .unwrap()
                        .to(move || generate_response(response.clone())),
                );
            }
        }

        app
    };

    println!("Starting web server.");
    HttpServer::new(app).bind(("127.0.0.1", 8080))?.run().await
}

fn get_web_method(method: &str) -> std::option::Option<Route> {
    match method {
        "GET" => Some(web::get()),
        "POST" => Some(web::post()),
        "PUT" => Some(web::put()),
        "PATCH" => Some(web::patch()),
        "DELETE" => Some(web::delete()),
        _ => None,
    }
}

fn get_json_files(folder: String) -> Vec<String> {
    let folder = folder.trim_matches('"').trim().to_string();
    let folder_path = Path::new(&folder);

    fs::read_dir(folder_path)
        .map(|entries| {
            entries
                .filter_map(|entry| {
                    let entry = entry.ok()?;
                    let path = entry.path();
                    if path.is_file() && path.extension().unwrap_or_default() == "json" {
                        Some(path.to_string_lossy().to_string())
                    } else {
                        None
                    }
                })
                .collect()
        })
        .unwrap_or_else(|err| {
            eprintln!("Error reading directory: {}", err);
            Vec::new()
        })
}
