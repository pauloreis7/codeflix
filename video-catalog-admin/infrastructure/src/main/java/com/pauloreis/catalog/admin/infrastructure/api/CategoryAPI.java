package com.pauloreis.catalog.admin.infrastructure.api;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.pauloreis.catalog.admin.domain.pagination.Pagination;

import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;

@RequestMapping(value = "categories")
public interface CategoryAPI {

  @PostMapping(consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
  ResponseEntity<?> createCategory();

  @GetMapping(consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
  Pagination<?> listCategories(
      @RequestParam(name = "search", required = false, defaultValue = "") final String search,
      @RequestParam(name = "page", required = false, defaultValue = "0") final int page,
      @RequestParam(name = "perPage", required = false, defaultValue = "10") final int perPage,
      @RequestParam(name = "sort", required = false, defaultValue = "name") final String sort,
      @RequestParam(name = "dir", required = false, defaultValue = "asc") final String direction);

}
